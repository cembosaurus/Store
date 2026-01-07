using Business.Metrics.DTOs;



namespace Business.Metrics.Services.Interfaces
{
    public interface IMetricsQueue
    {
        bool TryEnqueue(MetricsCreateDTO dto);
        ValueTask<MetricsCreateDTO> DequeueAsync(CancellationToken ct);
    }
}
