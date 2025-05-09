using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace HomeDelivery.Order.API.Controllers;

public class BaseController: ControllerBase
{
    protected IActionResult Return(Core.ResultResponses.IResult result)
    {
        return result.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(result),
            HttpStatusCode.NotFound => NotFound(result),
            HttpStatusCode.Unauthorized => Unauthorized(result),
            HttpStatusCode.InternalServerError => BadRequest(result),
            HttpStatusCode.MethodNotAllowed => BadRequest(result),
            HttpStatusCode.Forbidden => StatusCode( 403),
            HttpStatusCode.NotAcceptable => StatusCode(406, result),
            _ => Ok(result)
        };
    }
}