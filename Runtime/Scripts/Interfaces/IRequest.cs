namespace HHG.Messages.Runtime
{
    public interface IRequest<TResponse>
    {
        TResponse Response { get; set; }
    }
}