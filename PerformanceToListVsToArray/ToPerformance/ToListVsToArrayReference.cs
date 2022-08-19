namespace PerformanceToListVsToArray.ToPerformance;

public class ToListVsToArrayReference : ToListVsToArray<object>
{
    public ToListVsToArrayReference() : base(i => new
    {
        Id = i
    })
    {

    }
}