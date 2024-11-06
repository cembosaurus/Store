namespace Business.Metrics.DTOs
{
    public class HttpTransactionDTO
    {
        public int Id { get; set; }


        public ICollection<HttpRequestDTO> Requests { get; set; }
        public ICollection<HttpResponseDTO> Responses { get; set; }
    }
}
