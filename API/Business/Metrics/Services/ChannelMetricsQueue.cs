using Business.Metrics.DTOs;
using Business.Metrics.Services.Interfaces;
using System.Threading.Channels;


public sealed class ChannelMetricsQueue : IMetricsQueue
{

    private readonly Channel<MetricsCreateDTO> _channel;


    public ChannelMetricsQueue(int capacity = 1000)
    {
        _channel = Channel.CreateBounded<MetricsCreateDTO>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.DropWrite, // best-effort: drop if full
            SingleReader = true,
            SingleWriter = false
        });
    }




    public bool TryEnqueue(MetricsCreateDTO dto) => _channel.Writer.TryWrite(dto);


    public ValueTask<MetricsCreateDTO> DequeueAsync(CancellationToken ct)
        => _channel.Reader.ReadAsync(ct);
}