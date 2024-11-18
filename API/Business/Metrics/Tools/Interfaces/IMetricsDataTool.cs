using Microsoft.AspNetCore.Http;



namespace Business.Metrics.Tools.Interfaces
{
    public interface IMetricsDataTool
    {
        List<KeyValuePair<string, string[]>> HeadersToList(IHeaderDictionary headers);
        string ListToData(List<KeyValuePair<string, string[]>> data);
    }
}
