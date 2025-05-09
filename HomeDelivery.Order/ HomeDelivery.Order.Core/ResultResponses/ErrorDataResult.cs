using System.Net;

namespace HomeDelivery.Order.Core.ResultResponses
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        // sadece data

        // default data ve message
        public ErrorDataResult(string message, HttpStatusCode statusCode, List<string> errorMessages)
            : base(default!, false, message, statusCode, errorMessages)
        {
        }
        
        public ErrorDataResult(T data, string message, HttpStatusCode statusCode) 
            : base(data, false, message,
                statusCode)
        {
        }

        // default data ve message
        public ErrorDataResult(string message, HttpStatusCode statusCode) 
            : base(default!, false, message, statusCode)
        {
        }
        
      
        
       
    }
}
