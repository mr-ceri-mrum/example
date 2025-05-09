namespace HomeDelivery.Order.Core.Hash;

public interface IHashRepository
{
    string HashString(string text);
    bool VerifyHash(string text, string hashedString);
}

public class HashRepository(IEncoding encodingService) : IHashRepository
{
    
    public string HashString(string text)
    {
        return encodingService.Encrypt(text);
    }
    
    public bool VerifyHash(string text, string hashedString)
    {
        var isEncrypt = encodingService.Encrypt(text);
        return isEncrypt == hashedString;
    }
}