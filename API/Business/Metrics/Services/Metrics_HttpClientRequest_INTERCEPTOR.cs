using Business.Metrics.Services.Interfaces;
using Business.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Net;


public sealed class Metrics_HttpClientRequest_INTERCEPTOR : DelegatingHandler
{

    private readonly IHttpContextAccessor _accessor;
    private readonly string _thisService;
    private readonly Guid _appId;
    private sealed record CallState(int IndexOut, DateTime TimeOutUtc, string RequestFrom);


    public Metrics_HttpClientRequest_INTERCEPTOR(IHttpContextAccessor accessor, IConfiguration config)
    {
        _accessor = accessor;

        _appId = Metrics_MW.AppId_Model.AppId;

        var thisApp_Name = config.GetSection("Metrics:Name").Value;
        _thisService = !string.IsNullOrWhiteSpace(thisApp_Name)
            ? thisApp_Name
            : Path.GetFileNameWithoutExtension(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName)
              ?? "undefined";
    }





    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, CancellationToken cancellationToken)
    {
        var context = _accessor.HttpContext;
        var metricsData = context?.RequestServices.GetService(typeof(IMetricsData)) as IMetricsData;

        // resend request:
        if (metricsData is null)
            return await base.SendAsync(requestMessage, cancellationToken);

        var state = Request_OUT(requestMessage, metricsData);

        HttpResponseMessage? response = null;
        var createdFallbackResponse = false;

        try
        {
            // resend request:
            response = await base.SendAsync(requestMessage, cancellationToken);
            return response;
        }
        catch (HttpRequestException httpReqEx)
        {
            response = new HttpResponseMessage(httpReqEx.StatusCode ?? HttpStatusCode.ServiceUnavailable);
            createdFallbackResponse = true;
            throw;
        }
        finally
        {
            if (response is not null)
            {
                Response_IN(requestMessage, response, metricsData);

                AddHeaders(metricsData, requestMessage, response, state);

                if (!createdFallbackResponse)
                    AddHeadersFromResponse(metricsData, response);
            }
        }
    }


    //---------------------------------------------------------------------------------------------------------------------------------------


    private CallState Request_OUT(HttpRequestMessage requestMessage, IMetricsData metricsData)
    {
        var timeOutUtc = DateTime.UtcNow;

        // increase and read index passed from MW:
        var indexOut = ++metricsData.Index;

        requestMessage.Headers.Remove("Metrics.Index");
        requestMessage.Headers.TryAddWithoutValidation("Metrics.Index", indexOut.ToString());

        requestMessage.Headers.Remove("Metrics.RequestFrom");
        requestMessage.Headers.TryAddWithoutValidation("Metrics.RequestFrom", _thisService);

        return new CallState(indexOut, timeOutUtc, _thisService);
    }


    private void Response_IN(HttpRequestMessage requestMessage, HttpResponseMessage responseMessage, IMetricsData metricsData)
    {
        var timeInUtc = DateTime.UtcNow;

        var indexIn =
            responseMessage.Headers.TryGetValues("Metrics.Index", out var indexStrArr)
            && int.TryParse(indexStrArr?.FirstOrDefault(), out var idx)
                ? idx + 1
                : metricsData.Index + 1;

        responseMessage.Headers.Remove("Metrics.Index");

        var requestTo =
            responseMessage.Headers.TryGetValues("Metrics.ResponseFrom", out var responseFrom)
                ? responseFrom.FirstOrDefault() ?? ""
                : requestMessage.RequestUri?.Authority ?? "";

        responseMessage.Headers.Remove("Metrics.ResponseFrom");

        // passing Index back into MW:
        metricsData.Index = indexIn;

        // Store timestamp in metrics headers (kept compatible with your old AddHeaders)
        metricsData.AddHeader(
            $"Metrics.{_thisService}.{_appId}",
            $"{metricsData.Index}.RESP.IN.{requestTo}.{timeInUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}" +
            $"{((int)responseMessage.StatusCode == 503 ? ".HTTP:" + (int)responseMessage.StatusCode : "")}"
        );
    }


    //---------------------------------------------------------------------------------------------------------------------------------------


    private void AddHeaders(IMetricsData metricsData, HttpRequestMessage requestMessage, HttpResponseMessage responseMessage, CallState state)
    {
        var requestTo =
            responseMessage.Headers.TryGetValues("Metrics.ResponseFrom", out var responseFrom)
                ? responseFrom.FirstOrDefault() ?? ""
                : requestMessage.RequestUri?.Authority ?? "";

        metricsData.AddHeader(
            $"Metrics.{_thisService}.{_appId}",
            $"{state.IndexOut}.REQ.OUT.{requestTo}.{state.TimeOutUtc.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture)}"
        );
    }



    private void AddHeadersFromResponse(IMetricsData metricsData, HttpResponseMessage responseMessage)
    {
        var previousServices_MetricsHeaders = responseMessage.Headers.Where(h => h.Key.StartsWith("Metrics."));
        if (previousServices_MetricsHeaders.Any())
            metricsData.AddHeaders(previousServices_MetricsHeaders);

        var previousServices_AppIdHeaders = responseMessage.Headers.Where(h => h.Key.StartsWith("AppId."));
        if (previousServices_AppIdHeaders.Any())
            metricsData.AddHeaders(previousServices_AppIdHeaders);
    }
}
