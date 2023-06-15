---
layout: section
---

# Producing an enumerable!

---

# Interfaceless `Range`

```csharp {all} {maxHeight:'400px'}
public static class MyEnumerable
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
```

<!-- 

https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmuoDNoYBDMACwAoA3VqAAQBLDDAC2wvAICyATwCieHGJhRWwADYwAdACVWeAOYwuAVjQDaZAJTXqAhwKK0AnFxHjrAbgY0AzE60SE4kMgpKKmqaMNQA3vaORAHOwfpGMIrKqupaAmnG7ngYAgDOGPwYFkJFApD4GHZUjs0CALwAfAJ4MADueQbGmZE5JmUVFnVF3r7NSSH96UPZ0QkO8U0tDrCsACYQeBqyksVjUJUCMHg7Phubqy1z+RkRy1qFJ+VnVTWTDffN602mwwHCEJW0p2KrVKnwwNyBLRBYO0lx2bRhFQEAGpahB6vCgQBfGZAx4DZ5ZNQYaACADiMAwSyp0C41gE/yBHS6vQWgxezKgXEhFlR0yoHIcc1IvIpw2pUAlAkBCMc1WKYBwUFgRQJKoE2z2ByOaouV11KsVswCTyZrHl7wxX2Opp2jT1a0tQKR4I1WsuUMdxTglnN7oE3pRV3RqNDRM9DnjTgCJoAwprtVDOr6M7HNnNgBAIBoZBAeDAAHIwBAYVltTpYrHZ/0CAA8LtzAmJt071EJQA

-->

---

# Interfaced `Range`

```csharp {all} {maxHeight:'400px'}
public static class MyEnumerable
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
```

<!-- 

https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AYQgBtuYwMASwh4AzgG4GzViklVqAM2gwAhmAAWACgBuKqE0EYYAWwN4mAWQCeAUTw5jMKCuB82AJRV4A5jE0BWNCZaMgBKUOomKJZaAE5NQxNQuQYAZhikFhJLW3tHZ1cYagBvSOiidNZM1lSAHkE8DAA+Jk8fPwaMJlEMPQwgzqZIfAwIqmiJpgBeFrwYAHdWr187BycXPk0evqDhxuTqMqjSJfbV/I2YJhAYus6mo6ZS8cmo2BUAExFuKzMu7ag/SYMDwHzkryijwmFVOKzy60KCUa3V6gIGyL2oyh0WeEMmGHUglEbABXSmKL64Lx0QJRLYII+0wpgKYAGohhARlTXgBfQ4vV4wgCS53WGGg9UaLQA4jAMKLnOKoJpQkxsa8Zkw5os2nC1oroFtUUCGQcBZN1UwRfCDfoamxZfKbSolSq1ea8ZrHQqXYazZaTrqYD6ldcrSGJfdLbjqVFBmAcFBYI1udT3l88D8/sDQam8ZboekgxHlYNSeiuqaCzjq69acSE0mQWTmV04ME87GmPX6aCmQzO7za+6uyx0oMOInk2SWo3p4PJhBgAArfhda3631QThT5vTWe7lPD4cw4AQHiWCDaGAAORgCAwbs1rNZc73tRzYJPlRQrRgojlJ8WgJKAIEWbUmBvCAMAAZRwAAHeDoCMD4bAQMAYHgoQRBVBdCxYX8ABEiSQgC3WKPkPWiSiJkonkgA

-->

---

# Method calls

- Reference types (class)
  - v-table lookup (slow)
- Value types (struct)
  - direct call (fast)

A value type cast to an interface is boxed, becomes a reference type!

<!--
https://sharplab.io/#v2:C4LglgNgPgAgTARgLAChUDcCGAnABNgUwDMDCA7AYwIBUBPABwNwF5cyCB3XAJWNIMo0GBABQBKANwYcuLBACuQxizadcANUwKloydLwAjAPYAPAgBMVIgLK0AkmWCkimKmLmK6jKWhQwEAJwihCTkVF4EAHQA4gTAmtrien6BIh46MXEJikk+/kHGZuaZ8Vo5Ysmo8Dx8YTq4ILi2Dk7YLlSoAN6ouL24MADMuGCOuLGliWIsAHy4AAw+AL6oqADOwNjyFMAaZfWNzY7OrgRdPX2Dw6Pj2boz80srKCOt7UyHrydnKH1XOzd7cRLIA=
-->

---

# value-type enumerator `Range``

```csharp {all} {maxHeight:'400px'}
public static class MyEnumerable
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
```

<!-- 

https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AYQgBtuYwMASwh4AzgG4GzViklVqAM2gwAhmAAWACgBuKqE0EYYAWwN4mAWQCeAUTw5jMKCuB82AJRV4A5jE0BWNCZaMgBKUOomKJZaAE5NQxNQuQYAZhikFhJLW3tHZ1cYagBvSOiidNZMzx8YOwcnFz4mGt8EvAwmUQw9DCDBDqZIfAwIqmiJpgBeAD4mPBgAdxavX3r8pr9u3qDhjuTqMqiKrJXa9cbCphAY1IAeAYwZo6ZS8cmo2BUAExFuKzMnW2UD6TBgeG+cg+UReExOrTqeUufHaQJ6IP6gz2o1h0Te0MmGHUglEbGBnSmXXRGChBOiRJJbHB32mVN6TAA1EMICNaR8AL6HeTvD7w1aIhrODDQJgAcRgGAuUugmlCTFxH1m8yWZzWSOVUE05KCzIOIsmGqYAEklSppVAHh05qx7o8Zmx5Yr9XaVWM6dEtZ7bfbVXyLeaJjbvfbbh6FcHfeqI9DA/Ho76UsnjulurgBLqJRsYzco5KfQ63Zb8f7AUMcFBYB0wwSvr88P9a8zm9DLXD0giE4bHmyMZ2IX6a9Wa/TiaSwPXGxSR504MFu/6GaTmayu73ooKs+HpydhxwF+CKXN5w2L+vJhBgAArfidUtF6Ccc+DLXXxd3iZ7tmTDABAPCWBA2gwAAcjACAYKq0xzByHK/heTB3GCEL/uUlQoC0MCiAqCFakSUAQMsCzLFBEAYAAyjgAAODHQEY3w2AgYAwAxQgiKGgEsLhTAACIksxhEIcUB4ElJ+7UPyQA===

-->

---

# yield `Range`

```csharp {all}
public static class MyEnumerable
{
    public static IEnumerable<int> Range(int start, int count)
    {
        var end = start + count;
        for(var value = start; value < end; value++)
            yield return value;
    }
}
```

<!-- 

https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaUBua6gM2hgEMwACwAUAN35QmASwwwAtjLxMAsgE8AonhzyYUfsAA2MAHQAlfngDmMEQFY0TWmQCUL6k08taAThGyFF04qBgBmbyQWElVNbV19IxhqAG8PLyJw1kjWUIAeaTwMAD4mC2tbAowmAGcMSQxHSqZIfAx3Ki8mVI7OrwkpGDwAEyYAXhq6qCqAamaIVuDer14ocUkmCUMcGDGJ+vYN/i2d3KZBoYPN7enp9qWl1loWAHZD48WvAF9qT6A=

-->

---

# Benchmarks

|              Method | Count |         Mean |     Error |    StdDev |        Ratio | RatioSD |   Gen0 | Allocated | Alloc Ratio |
|-------------------- |------ |-------------:|----------:|----------:|-------------:|--------:|-------:|----------:|------------:|
|       **Interfaceless** |     **0** |    **11.337 ns** | **0.0765 ns** | **0.0716 ns** |     **baseline** |        **** | **0.0076** |      **48 B** |            **** |
|          Interfaced |     0 |    13.175 ns | 0.0092 ns | 0.0086 ns | 1.16x slower |   0.01x | 0.0076 |      48 B |  1.00x more |
| ValueTypeEnumerable |     0 |     4.258 ns | 0.0083 ns | 0.0078 ns | 2.66x faster |   0.02x | 0.0038 |      24 B |  2.00x less |
|               Yield |     0 |    15.791 ns | 0.0148 ns | 0.0124 ns | 1.39x slower |   0.01x | 0.0089 |      56 B |  1.17x more |
|                     |       |              |           |           |              |         |        |           |             |
|       **Interfaceless** |     **1** |    **11.298 ns** | **0.0075 ns** | **0.0071 ns** |     **baseline** |        **** | **0.0076** |      **48 B** |            **** |
|          Interfaced |     1 |    15.942 ns | 0.0166 ns | 0.0155 ns | 1.41x slower |   0.00x | 0.0076 |      48 B |  1.00x more |
| ValueTypeEnumerable |     1 |     4.393 ns | 0.0120 ns | 0.0113 ns | 2.57x faster |   0.01x | 0.0038 |      24 B |  2.00x less |
|               Yield |     1 |    21.920 ns | 0.0727 ns | 0.0645 ns | 1.94x slower |   0.01x | 0.0089 |      56 B |  1.17x more |
|                     |       |              |           |           |              |         |        |           |             |
|       **Interfaceless** |    **10** |    **20.089 ns** | **0.0321 ns** | **0.0285 ns** |     **baseline** |        **** | **0.0076** |      **48 B** |            **** |
|          Interfaced |    10 |    61.251 ns | 0.4010 ns | 0.3348 ns | 3.05x slower |   0.02x | 0.0076 |      48 B |  1.00x more |
| ValueTypeEnumerable |    10 |    10.169 ns | 0.0157 ns | 0.0147 ns | 1.98x faster |   0.00x | 0.0038 |      24 B |  2.00x less |
|               Yield |    10 |    66.130 ns | 0.0318 ns | 0.0297 ns | 3.29x slower |   0.00x | 0.0088 |      56 B |  1.17x more |
|                     |       |              |           |           |              |         |        |           |             |
|       **Interfaceless** |  **1000** | **1,946.993 ns** | **2.5609 ns** | **2.2701 ns** |     **baseline** |        **** | **0.0076** |      **48 B** |            **** |
|          Interfaced |  1000 | 3,659.891 ns | 2.2881 ns | 2.0283 ns | 1.88x slower |   0.00x | 0.0076 |      48 B |  1.00x more |
| ValueTypeEnumerable |  1000 |   593.285 ns | 0.9796 ns | 0.9164 ns | 3.28x faster |   0.01x | 0.0038 |      24 B |  2.00x less |
|               Yield |  1000 | 4,682.071 ns | 8.2851 ns | 7.3445 ns | 2.40x slower |   0.00x | 0.0076 |      56 B |  1.17x more |

---

# Performance

- Collection like `List<T>` use the value-type optimization
  - If you need performance, don't cast it to an interface
- If you implement a collection type and can't control the use cases (library development)
  - Favor the implementation using value-type enumerator