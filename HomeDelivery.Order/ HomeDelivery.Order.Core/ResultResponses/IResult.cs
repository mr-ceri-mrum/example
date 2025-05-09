using System.Net;

namespace HomeDelivery.Order.Core.ResultResponses
{
    public interface IResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Result { get; }
        public string Message { get; }
        public string? ItemId { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
