namespace HHG.Messages
{
    public interface IAggregate<R>
    {
        R Aggregate(R a, R b);
    }
}