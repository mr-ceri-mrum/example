namespace HomeDelivery.Order.Core.ResultResponses
{
    public interface IDataResult<T> : IResult
    {
        T Data { get; }
    }
}
