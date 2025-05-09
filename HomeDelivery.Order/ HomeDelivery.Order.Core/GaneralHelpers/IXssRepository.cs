using Ganss.Xss;

namespace HomeDelivery.Order.Core.GaneralHelpers;

public interface IXssRepository
{
    string Execute(string text);
}

public class XssRepository : IXssRepository
{
    public string Execute(string text)
    {
        var sanitizer = new HtmlSanitizer();
        var sanitized = sanitizer.Sanitize(text, "");
        return sanitized;
    }
}