---
layout: section
---

# Linq Tips!

---

# Linq

- A collection of extension methods for:
  - `IEnumerable<T>`
  - `IAsyncEnumerable<T>`
  - `IQueryable<T>`
  - `IAsyncQueryable<T>`

---

# Linq

## `IEnumerable<T>` & `IAsyncEnumerable<T>`

Local processing

## `IQueryable<T>` & `IAsyncQueryable<T>`

Delegates processing to database, GPU or other dataprocessing mechanisms

---
layout: center
---

# Count()

---

# Average (don't do this)

```csharp {all|1|3-4|10|all}
public static double? Average(this IEnumerable<int> enumerable) 
{
    if(enumerable.Count() == 0)
        return null;

    var sum = 0;
    foreach (var value in enumerable)
        sum += value;

    return sum / (double)enumerable.Count();
}
```

<div v-click="4">
Iterates the collection 3 times! 
</div>

---

# Count()

```csharp
public static int Count<T>(this IEnumerable<T> enumerable) 
{
    var count = 0;
    using(var enumerator = enumerable.GetEnumerator())
    {
        while (enumerator.MoveNext())
            count++;
    }
    return count;
}
```

<div v-click="1">
Linq contains optimizations for some types of collections!
</div>

---

# Average 

```csharp {all|3-9|11-12}
public static double? Average(this IEnumerable<int> enumerable) 
{
    var sum = 0;
    var count = 0;
    foreach (var value in enumerable)
    {
        sum += value;
        count++;
    }
    
    if(count == 0)
        return null;

    return sum / (double)count;
}
```

---

# Average (IReadOnlyCollection&lt;T&gt;)

```csharp
public static double? Average(this IReadOnlyCollection<int> enumerable) 
{
    if (enumerable.Count == 0)
        return null;
    
    var sum = 0;
    foreach (var value in enumerable)
        sum += value;
    
    return sum / (double)enumerable.Count;
}
```

---
layout: center
---

# Any()

---

# Any()

```csharp {all|5}
public static bool Any<T>(this IEnumerable<T> enumerable) 
{
  using (var enumerator = enumerable.GetEnumerator())
  {
      return enumerator.MoveNext();
  }
}
```

<div v-click="1">
Calls `MoveNext()` only once!
</div>

--- 

# Any() (usage)

```csharp
public void DoSomething<T>(IEnumerable<T> enumerable) 
{
    if (!enumerable.Any())
        return;

    // do something here
}
```

---
layout: center 
---

#ToList()/ToArray()

--- 

# ToList()/ToArray() (don't do this)

```csharp
var numbers = Enumerable.Range(0, 10)
    .ToList();

var evenNumbers = numbers
    .Where(number => (number & 0x01) == 0)
    .ToList();

foreach(var number in evenNumbers)
    Console.WriteLine(number);
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AGQEs8BHAbmrUAbgEMoTPDgC2wGFADOTALxMAolOnzRwADYw2AJVF4A5jAAUZNE1pkAlNSbOmbACoRuCjBfuCqIuJMMMIweAByMnKKKpJR8gpOLmwA6gAW8paa0SoAfEwW2fJMAGRMZAhktPYqqg5Jzu6eXN6+/tQAZtAwomBpFmISRRI8waER8YqOVC4stACchZN+1EA==)

---

# ToList()/ToArray() (don't do this)

```csharp
Enumerable.Range(0, 10)
    .ToList()
    .ForEach(Console.WriteLine);
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AGQEs8BHAbmrUAonhwBbGFACGwADYw2AJWl4A5jAAUZNE1pkAlNSYmmbACoRuAZwyajVU2YBi0YdLAALTawCcB/iA==)

---

# ToList()

```csharp {all|4-5}
    public static List<T> ToList<T>(this IEnumerable<T> source)
    {
        var list = new List<T>();
        foreach(var item in source)
            list.Add(item);
        return list;
    }
```

---

# Lazy evaluation

```csharp
var numbers = Enumerable.Range(0, 10);

var evenNumbers = numbers
    .Where(number => (number & 0x01) == 0);

foreach(var number in evenNumbers)
    Console.WriteLine(number);
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AGQEs8BHAbmrUAbgEMoTPDgC2wGFADOTALxMAolOnzRwADYw2AJVF4A5jAAUZNE1pkAlIKojxTGMJh4AcjLmKVkr7yCtRMYUxsAOoAFvKWmn4qAHxMFgnyTABkTGQIZLT2KqoOTtQAZtAwomDRFmIS6RI8bh7eQYr2oeGsAJxp7Y7UQA===)

---

# ForEach

```csharp
public static class MyEnumerable
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach(var item in source)
            action(item);
    }
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaUBuB51gOgBkAlngCOnKtQCieHAFsYUAIbAANjB4AlBXgDmMABRk0TWmQCUPAGLQJCsAAs9rAJymxDAMwtaSFiSYBZAE8pWXklVWoAb2omWJZPVh8iFCYrKBt7AB4AFQA+PQw7AQBnL3cc3KZiiBwoMBgjVhIKplsMAQg8Uxi46Ko4gaYAM2gYWwcANwUoJgEMGBlZvCqauphu/sGtto68PTmF1x7YgF9qE6A=)



