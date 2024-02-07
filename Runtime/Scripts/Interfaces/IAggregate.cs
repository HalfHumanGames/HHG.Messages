namespace HHG.Messages
{
    public interface IAggregate<TSource>
    {
        TSource Aggregate(TSource a, TSource b);
    }

    public interface IAggregate<TSource, TAccumulate>
    {
        TAccumulate GetSeed() => default;

        TAccumulate Aggregate(TAccumulate a, TSource b);
    }

    public interface IAggregate<TSource, TAccumulate, TResult> : IAggregate<TSource, TAccumulate>
    {
        TResult GetResult(TAccumulate a);
    }
}