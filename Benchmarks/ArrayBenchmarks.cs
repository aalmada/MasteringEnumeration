using BenchmarkDotNet.Attributes;
using System.Collections;
using System.Collections.Generic;

public class ArrayBenchmarks
{
    int[]? array;

    [Params(0, 1, 10, 1_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void GlobalSetup()
    {
        array = Enumerable.Range(0, Count).ToArray();
    }

    [Benchmark(Baseline = true)]
    public int ArrayFor()
    {
        var sum = 0;
        for (var index = 0; index < Count; index++)
            sum += array![index];
        return sum;
    }   

    [Benchmark]
    public int ArrayForeach()
    {
        var sum = 0;
        foreach (var item in array!)
            sum += item;
        return sum;
    }   

    [Benchmark]
    public int SpanFor()
    {
        var span = array!.AsSpan();
        var sum = 0;
        for (var index = 0; index < Count; index++)
            sum += span[index];
        return sum;
    }   

    [Benchmark]
    public int SpanForeach()
    {
        var span = array!.AsSpan();
        var sum = 0;
        foreach (var item in span)
            sum += item;
        return sum;
    }
}
