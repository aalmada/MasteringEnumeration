---
layout: section
---

# Array, Memory & Span

---

# Array, Memory & Span

- Pull
- Random
- Special treatment by the JIT compiler

--- 

# `for` loop

```csharp
static int Sum(int[] source)
{
    var sum = 0;
    for (var index = 0; index < source.Length; index++)
        sum += source[index];
    return sum;
}   
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA+ABATABgLABQ2AjIRjgAQbEB0AMgJYB2AjgNyGEBuAhlBXyg8AnhQC8FAKJMArgFsYQ4ABsYNAEo8mAcxgAKHABoKxHAEoaAFQgBBKEOF6zHIsQCcegMry9gkWedOVyQKZgAXCm85PXCAbQBdCgBnCBkoMBgzQgBvQgp8il5+JPlxChwXAooAM2gKPSLQpgATGAQyiqbW9oAeZNT0tToYHTCACzYutoBqaayCKqqSuQppiRS0jNjmbvjKgowAdmT5FwBfAqA===)

--- 

# `foreach` loop

```csharp
static int Sum(int[] source)
{
  var sum = 0;
  foreach (var item in source)
    sum += item;
  return sum;
}   
```

---

* Benchmarks 

|       Method | Count |        Mean |     Error |    StdDev |        Ratio | RatioSD | Allocated | Alloc Ratio |
|------------- |------ |------------:|----------:|----------:|-------------:|--------:|----------:|------------:|
|     **ArrayFor** |    **10** |   **8.0370 ns** | **0.0310 ns** | **0.0290 ns** |     **baseline** |        **** |         **-** |          **NA** |
| ArrayForeach |    10 |   3.9263 ns | 0.0192 ns | 0.0180 ns | 2.05x faster |   0.01x |         - |          NA |
|      SpanFor |    10 |   9.6291 ns | 0.0412 ns | 0.0386 ns | 1.20x slower |   0.01x |         - |          NA |
|  SpanForeach |    10 |   4.6778 ns | 0.0797 ns | 0.0746 ns | 1.72x faster |   0.03x |         - |          NA |
|              |       |             |           |           |              |         |           |             |
|     **ArrayFor** |  **1000** | **401.9290 ns** | **1.4809 ns** | **1.3852 ns** |     **baseline** |        **** |         **-** |          **NA** |
| ArrayForeach |  1000 | 323.1575 ns | 0.8017 ns | 0.7499 ns | 1.24x faster |   0.01x |         - |          NA |
|      SpanFor |  1000 | 326.9202 ns | 1.3916 ns | 1.3017 ns | 1.23x faster |   0.01x |         - |          NA |
|  SpanForeach |  1000 | 323.4267 ns | 1.5112 ns | 1.4136 ns | 1.24x faster |   0.01x |         - |          NA |

--- 

# `foreach` compiled

```csharp
```

