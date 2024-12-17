using Business.Metrics.Services.Interfaces;
using Business.Middlewares;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;



public class Metrics_HttpClientRequest_INTERCEPTOR : DelegatingHandler
{
    private IMetricsData _metricsData;
    private HttpRequestMessage _requestMessage;
    private HttpResponseMessage _responseMessage;
    private readonly string _thisService;
    private string _requestTo;
    private readonly Guid _appId;
    private int _indexOUT;
    private int _indexIN;
    private DateTime _timeOUT;
    private DateTime _timeIN;


    public Metrics_HttpClientRequest_INTERCEPTOR(IMetricsData metricsData, IConfiguration config)
    {
        _metricsData = metricsData;
        _appId = Metrics_MW.AppId_Model.AppId;
        _thisService = config.GetSection("Metrics:Name").Value!;
        _thisService = !string.IsNullOrWhiteSpace(_thisService) ? _thisService : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName) ?? "undefined";
        _requestMessage = new HttpRequestMessage();
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        _requestMessage = requestMessage;

        Request_OUT();


        try
        {
            _responseMessage = await base.SendAsync(requestMessage, cancellationToken);
        }
        catch (HttpRequestException httpReqEx)
        {
            _responseMessage = new HttpResponseMessage(httpReqEx.StatusCode ?? HttpStatusCode.ServiceUnavailable);

            throw;
        }
        finally
        {
            Response_IN();
            AddHeaders();
        }


        AddHeadersFromResponse();

        return _responseMessage;
    }




    private void Request_OUT()
    {
        // metrics END:

        _timeOUT = DateTime.Now;

        // increase and read index passed from MW:
        _indexOUT = ++_metricsData.Index;

        // send index value to called app ot maintain continual incrementation:
        _requestMessage.Headers.Remove("Metrics.Index");
        _requestMessage.Headers.Add("Metrics.Index", _indexOUT.ToString());

        // send name of this app to called app:
        _requestMessage.Headers.Remove("Metrics.RequestFrom");
        _requestMessage.Headers.Add("Metrics.RequestFrom", _thisService);
    }



    private void Response_IN()
    {
        // metrics START:

        _timeIN = DateTime.Now;

        // if no index is returned from service response (503 or any 5xx ex) then copy indexOUT into indexIN and increase it by 1 to maintain continuity:
        _indexIN = _responseMessage.Headers.TryGetValues("Metrics.Index", out IEnumerable<string>? indexStrArr) 
            ? int.TryParse(indexStrArr?.ElementAt(0), out int indexInt) ? ++indexInt : _indexOUT + 1
            : _indexOUT + 1;
        _responseMessage.Headers.Remove("Metrics.Index");

        _requestTo = _responseMessage.Headers.TryGetValues("Metrics.ResponseFrom", out IEnumerable<string>? responseFrom) ? responseFrom.ElementAt(0) : _requestMessage.RequestUri?.Authority ?? "";
        _responseMessage.Headers.Remove("Metrics.ResponseFrom");

        // passing Index back into MW:
        _metricsData.Index = _indexIN;
    }


    private void AddHeaders()
    {
        _metricsData.AddHeader(
            $"Metrics.{_thisService}.{_appId}",
            $"{_indexOUT}.REQ.OUT.{_requestTo}.{_timeOUT.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
            );

        _metricsData.AddHeader(
            $"Metrics.{_thisService}.{_appId}",
            $"{_indexIN}.RESP.IN.{_requestTo}.{_timeIN.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}.{_responseMessage.StatusCode}.{(int)_responseMessage.StatusCode}"
            );
    }


    private void AddHeadersFromResponse()
    {
        // Appending Metrics headers arrived from responses:

        var previousServices_MetricsHeaders = _responseMessage.Headers.Where(h => h.Key.StartsWith($"Metrics."));

        if (previousServices_MetricsHeaders != null)
        {
            _metricsData.AddHeaders(previousServices_MetricsHeaders);
        }

        var previousServices_AppIdHeaders = _responseMessage.Headers.Where(h => h.Key.StartsWith($"AppId."));

        if (previousServices_AppIdHeaders != null)
        {
            _metricsData.AddHeaders(previousServices_AppIdHeaders);
        }
    }

}