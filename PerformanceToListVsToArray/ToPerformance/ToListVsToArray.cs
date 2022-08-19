using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace PerformanceToListVsToArray.ToPerformance;

[SimpleJob(RuntimeMoniker.Net60), SimpleJob(RuntimeMoniker.Net48)]
[MemoryDiagnoser, KeepBenchmarkFiles]
public class ToListVsToArray<T>
{
    private readonly Func<int, T> _mapper;

    private T[] _entries = null!;

    protected ToListVsToArray(Func<int, T> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [Params(10, 100, 1000, 10000)]
    public int Length { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _entries = Enumerable.Range(0, Length).Select(_mapper).ToArray();
    }

    [Benchmark(Baseline = true)]
    public T[] Copy_Array()
    {
        var result = new T[_entries.Length];
        _entries.CopyTo(result, 0);
        return result;
    }

    [Benchmark]
    public List<T> Copy_List() => new(_entries);

    [Benchmark]
    public T[] Array_ToArray() => _entries.ToArray();

    [Benchmark]
    public List<T> Array_ToList() => _entries.ToList();

    [Benchmark]
    public T[] Enumerable_ToArray() => EntriesEnumerable().ToArray();

    [Benchmark]
    public List<T> Enumerable_ToList() => EntriesEnumerable().ToList();

    private IEnumerable<T> EntriesEnumerable()
    {
        foreach (var s in _entries.AsEnumerable())
            yield return s;
    }
}