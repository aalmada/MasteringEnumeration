---
layout: section
---

# IEnumerable / IEnumerator
 
---

# IEnumerable

```csharp {all|1|3}
public interface IEnumerable
{ 
  IEnumerator GetEnumerator();
}
```

---
layout: statement
---

## IEnumerable is a factory of enumerators!

Each new enumeration creates a new instance of the enumerator

---

# IEnumerator

```csharp {all|1|5|3|6}
public interface IEnumerator
{
    object Current { get; }
    
    bool MoveNext();
    void Reset();
}
```

---
layout: statement
---

## IEnumerator abstracts a stream where data can be **pulled** from!

Enumerator is state machine that keeps the state of the enumeration

---

# IEnumerable

- Pull
- Read-only 
- Sequential

---

# Generics support

- `MoveNext()` returns `object`
- Requires casting at runtime
- Slow

---

# Generics support

```csharp {all|1|2|4|all}
public interface IEnumerable<out T> 
  : IEnumerable
{
    new IEnumerator<T> GetEnumerator();
}
```

```csharp {all|1|2|3|5}
public interface IEnumerator<out T> 
  : IEnumerator
  , IDisposable
{
    new T Current { get; }
}
```

---

# Async support

- If data is not in memory:
  - Hard drive
  - Database
  - Remote location
- `MoveNext()` call will block execution

---

# Async support

```csharp {all|1|3|all}
public interface IAsyncEnumerable<out T>
{ 
  IAsyncEnumerator<out T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
}
```

```csharp {all|1|2|6|4}
public interface IAsyncEnumerator<out T> 
    : IAsyncDisposable
{
    T Current { get; }
    
    ValueTask<bool> MoveNextAsync();
}
```

---
layout: statement
---

## Any type that implements `IEnumerable` can be enumerated using `foreach`

---

# ForEach

```csharp
public static int Sum(IEnumerable<int> source)
{
    var sum = 0;
    foreach (var item in source)
        sum += item;
    return sum;
}
```
