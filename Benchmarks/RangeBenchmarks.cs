using BenchmarkDotNet.Attributes;
using System.Collections;
using System.Collections.Generic;

public class SumBenchmarks
{
    [Params(0, 1, 10, 1_000)]
    public int Count { get; set; }

    [Benchmark(Baseline = true)]
    public int Interfaceless()
    {
        var sum = 0;
        foreach (var angle in InterfacelessImpl.Range(0, Count))
            sum += angle;
        return sum;
    }   

    [Benchmark]
    public int Interfaced()
    {
        var sum = 0;
        foreach (var angle in InterfacedImpl.Range(0, Count))
            sum += angle;
        return sum;
    }   

    [Benchmark]
    public int ValueTypeEnumerable()
    {
        var sum = 0;
        foreach (var angle in ValueTypeEnumerableImpl.Range(0, Count))
            sum += angle;
        return sum;
    }   

    [Benchmark]
    public int Yield()
    {
        var sum = 0;
        foreach (var angle in YieldImpl.Range(0, Count))
            sum += angle;
        return sum;
    }   
}

public static class InterfacelessImpl
{
    public static RangeEnumerable Range(int start, int count)
        => new RangeEnumerable(start, count);

    public class RangeEnumerable
    {
        readonly int start, end;
        
        public RangeEnumerable(int start, int count)
        {
            this.start = start;
            this.end = start + count;
        }

        public RangeEnumerator GetEnumerator() 
            => new RangeEnumerator(start, end);

        public class RangeEnumerator
        {
            int current;
            readonly int end;
            
            public RangeEnumerator(int start, int end)
            {
                this.current = start - 1;
                this.end = end;
            }
            
            public int Current => current;
            public bool MoveNext() => ++current < end;
        }
    }
}

public static class InterfacedImpl
{
    public static IEnumerable<int> Range(int start, int count)
        => new RangeEnumerable(start, count);

    class RangeEnumerable : IEnumerable<int>
    {
        readonly int start, end;
        
        public RangeEnumerable(int start, int count)
        {
            this.start = start;
            this.end = start + count;
        }

        public IEnumerator<int> GetEnumerator() 
            => new RangeEnumerator(start, end);
        
        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();

        class RangeEnumerator : IEnumerator<int>
        {
            int current;
            readonly int end;
            
            public RangeEnumerator(int start, int end)
            {
                this.current = start - 1;
                this.end = end;
            }
            
            public int Current => current;
            object IEnumerator.Current => current;
            
            public bool MoveNext() => ++current < end;
            public void Reset() => throw new NotSupportedException();
            public void Dispose() {}
        }
    }
}

public static class ValueTypeEnumerableImpl
{
    public static RangeEnumerable Range(int start, int count)
        => new RangeEnumerable(start, count);

    public class RangeEnumerable : IEnumerable<int>
    {
        readonly int start, end;
        
        public RangeEnumerable(int start, int count)
        {
            this.start = start;
            this.end = start + count;
        }


        public RangeEnumerator GetEnumerator() 
            => new RangeEnumerator(start, end);
        
        IEnumerator<int> IEnumerable<int>.GetEnumerator()
            => GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();

        public struct RangeEnumerator : IEnumerator<int>
        {
            int current;
            readonly int end;
            
            public RangeEnumerator(int start, int end)
            {
                this.current = start - 1;
                this.end = end;
            }
            
            public int Current => current;
            object IEnumerator.Current => current;
            
            public bool MoveNext() => ++current < end;
            public void Reset() => throw new NotSupportedException();
            public void Dispose() {}
        }
    }
}

public static class YieldImpl
{
    public static IEnumerable<int> Range(int start, int count)
    {
        var end = start + count;
        for(var value = start; value < end; value++)
            yield return value;
    }
}