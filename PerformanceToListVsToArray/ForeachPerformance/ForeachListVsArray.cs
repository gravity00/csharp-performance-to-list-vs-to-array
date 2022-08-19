using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceToListVsToArray.ForeachPerformance;

[SimpleJob(RuntimeMoniker.Net60), SimpleJob(RuntimeMoniker.Net48)]
[MemoryDiagnoser, KeepBenchmarkFiles]
public class ForeachListVsArray<T>
{
    private readonly Func<int, T> _mapper;

    private T[] _entriesArray = null!;
    private List<T> _entriesList = null!;

    protected ForeachListVsArray(Func<int, T> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [Params(10, 100, 1000, 10000)]
    public int Length { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _entriesArray = new T[Length];
        _entriesList = new List<T>(Length);
        for (var i = 0; i < Length; i++)
        {
            var e = _mapper(i);

            _entriesArray[i] = e;
            _entriesList.Add(e);
        }
    }

    [Benchmark(Baseline = true)]
    public void Array()
    {
        foreach (var _ in _entriesArray)
        {

        }
    }

    [Benchmark]
    public void List()
    {
        foreach (var _ in _entriesList)
        {

        }
    }

    [Benchmark]
    public void Enumerable_Array()
    {
        foreach (var _ in _entriesArray.AsEnumerable())
        {

        }
    }

    [Benchmark]
    public void Enumerable_List()
    {
        foreach (var _ in _entriesList.AsEnumerable())
        {

        }
    }
}