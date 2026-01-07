using Business.Metrics.DTOs;
using Business.Metrics.Http.Services.Interfaces;
using Business.Metrics.Services.Interfaces;
using Business.Tools;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Business.Enums;



public sealed class Metrics_Worker : BackgroundService
{

    private readonly IMetricsQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ConsoleWriter _cw;


    public Metrics_Worker(IMetricsQueue queue, IServiceScopeFactory scopeFactory, ConsoleWriter cw)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _cw = cw;
    }




    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            MetricsCreateDTO dto;

            try
            {
                dto = await _queue.DequeueAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var http = scope.ServiceProvider.GetRequiredService<IHttpMetricsService>();

                using var cts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                cts.CancelAfter(TimeSpan.FromSeconds(2)); // keep telemetry cheap

                await http.UpdateAsync(dto, cts.Token);
            }
            catch (Exception ex)
            {
                _cw.Message("Metrics send failed: ", "MetricsService", "", TypeOfInfo.FAIL, ex.Message);
            }
        }

    }

}
