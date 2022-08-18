using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<PrimitiveToListVsToArray>();
BenchmarkRunner.Run<ReferenceToListVsToArray>();

[SimpleJob(RuntimeMoniker.Net60), SimpleJob(RuntimeMoniker.Net48)]
[MemoryDiagnoser]
public class ToListVsToArray<T>
{
    private readonly Func<int, T> _mapper;
    private T[] _classEntries = null!;

    protected ToListVsToArray(Func<int, T> mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [Params(10, 100, 1000, 10000)]
    public int Length { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        _classEntries = Enumerable.Range(0, Length).Select(_mapper).ToArray();
    }

    [Benchmark(Baseline = true)]
    public T[] Copy_Array()
    {
        var result = new T[_classEntries.Length];
        _classEntries.CopyTo(result, 0);
        return result;
    }

    [Benchmark]
    public List<T> Copy_List() => new(_classEntries);

    [Benchmark]
    public T[] Array_ToArray() => _classEntries.ToArray();

    [Benchmark]
    public List<T> Array_ToList() => _classEntries.ToList();

    [Benchmark]
    public T[] Enumerable_ToArray()
    {
        var enumerable = ForceEnumerable(_classEntries);
        return enumerable.ToArray();
    }

    [Benchmark]
    public List<T> Enumerable_ToList()
    {
        var enumerable = ForceEnumerable(_classEntries);
        return enumerable.ToList();
    }

    private static IEnumerable<T> ForceEnumerable(IEnumerable<T> source)
    {
        using var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
            yield return enumerator.Current;
    }
}

public class PrimitiveToListVsToArray : ToListVsToArray<int>
{
    public PrimitiveToListVsToArray() : base(i => i)
    {

    }
}

public class ReferenceToListVsToArray : ToListVsToArray<ReferenceToListVsToArray.DummyReference>
{
    public ReferenceToListVsToArray() : base(i => new DummyReference
    {
        Id = i
    })
    {

    }

    public class DummyReference
    {
        public int Id { get; set; }
    }
}