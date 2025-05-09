namespace HomeDelivery.Order.Core.GaneralHelpers;

public static class MathHelper
{
    public static async Task<int> GenerateCodeForOrder(this int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        var random = new Random();
        value = random.Next(1, 9999); 
        return await Task.FromResult(value);
    }
    
    public static async Task<int> GenerateCodeForOrder()
    {
        var random = new Random();
        var value = random.Next(1, 9999);
        return await Task.FromResult(value);
    }
}