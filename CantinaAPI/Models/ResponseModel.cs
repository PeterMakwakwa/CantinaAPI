using System.Net;

namespace CantinaAPI.Models
{
    public class ResponseModel<TResult>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public TResult? Model { get; set; }

    }
}
