namespace KobaParts.Models.Api
{
    public class BaseResponse<T>
    {
        public string Description { get; set; } = string.Empty;
        public int StatusCode { get; set; } = 0;
        public int TotalRecords { get; set; } = 0;
        public List<T>? Values { get; set; }
    }
}
