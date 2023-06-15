---
layout: section
---

# Consuming an enumerable!

---

# `foreach`

```csharp {all|4}
public static int Sum(IEnumerable<int> enumerable) 
{
  var sum = 0;
  foreach (var number in enumerable)
    sum += number;
  return sum;
}
```

---

# `await foreach`

```csharp {all|4}
public static async ValueTask<int> SumAsync(IAsyncEnumerable<int> enumerable, CancellationToken token = default) 
{
  var sum = 0;
  await foreach(var number in enumerable.WithCancellation(token))
    sum += number;
  return sum;
}
```

---

# compiled `foreach`

```csharp {all|4|6|7}
public static int Sum(IEnumerable<int> enumerable)
{
  int num = 0;
  using(var enumerator = enumerable.GetEnumerator())
  {
    while (enumerator.MoveNext())
      num += enumerator.Current;
  }
  return num;
}
```

<div v-click="1">
`IEnumerator&lt;T&gt;` implements `IDisposable`. `using` disposes enumerator on exit.
</div>

<div v-click="4">
Does not check for `null`. `Enumerable&lt;T&gt;` should be used when there's nothing to return. 
</div>

---

# compiled `await foreach`

```csharp {all|4|6|7}
public static async ValueTask<int> SumAsync(IAsyncEnumerable<int> enumerable, CancellationToken token = default)
{
  int sum = 0;
  await using (var enumerator = enumerable.GetAsyncEnumerator(token))
  {
    while (await enumerator.MoveNextAsync())
      sum += enumerator.Current;
  }
  return sum;
}
```
