using System.Net;

namespace HomeDelivery.Order.Core.ResultResponses;

public class SuccessDataResult<T> : DataResult<T>
{
    public SuccessDataResult(T data, bool result, string message, HttpStatusCode statusCode, List<string> errorMessages) : base(data, result, message, statusCode, errorMessages)
    {
    }
    public SuccessDataResult(T data, string message) : base(
        data: data,
        result: true, 
        message: message, 
        statusCode: HttpStatusCode.OK) { }

    // data ve message  ve itemId
    public SuccessDataResult(T data, string message, string itemId) : base(
        data: data,
        result: true,
        message: message, 
        statusCode: HttpStatusCode.OK, 
        itemId: itemId) { }

    // data ve message  ve itemId
    public SuccessDataResult(T data, string message, string returnUrl, string itemId) : base(
        data: data,
        result: true,
        message: message,
        statusCode: HttpStatusCode.OK,
        itemId: itemId,
        returnUrl: returnUrl) { }

    // default data ve message
    public SuccessDataResult(string message) : base(
        data: default!,
        result: true,
        message: message,
        statusCode: HttpStatusCode.OK) { }
    
    // sadece default data
 
}