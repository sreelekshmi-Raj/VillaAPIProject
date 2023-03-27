using System.Net;

namespace VillaAPI.Models
{
    public class ResponseObject
    {
        public HttpStatusCode statusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }
}
