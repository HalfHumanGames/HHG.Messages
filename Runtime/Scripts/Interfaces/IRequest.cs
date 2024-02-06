namespace HHG.Messages
{
    public interface IRequest<TResponse>
    {
        TResponse Response { get; set; }
    }
}