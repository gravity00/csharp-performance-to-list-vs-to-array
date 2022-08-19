namespace PerformanceToListVsToArray.ForeachPerformance;

public class ForeachListVsArrayReference : ForeachListVsArray<object>
{
    public ForeachListVsArrayReference() : base(i => new
    {
        Id = i
    })
    {

    }
}