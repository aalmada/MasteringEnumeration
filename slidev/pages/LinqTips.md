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

In-memory processing.

## `IQueryable<T>` & `IAsyncQueryable<T>`

Delegates processing to a provider. Can be a database, GPU or any other data storage/processing mechanism.

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
layout: statement
---

If you need to know if a collection is empty

# use Any()

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

# First() vs. Single()

---

# First/FirstOrDefault

```csharp {all|5}
public static T First<T>(this IEnumerable<T> enumerable)
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        if(!enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains no elements");

        return enumerator.Current;
    }
}

public static T? FirstOrDefault<T>(this IEnumerable<T> enumerable)
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        if(!enumerator.MoveNext())
            return default;

        return enumerator.Current;
    }
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaUBua6gNwEMomAZwg4oYGEwC8TPDADuTAJZ4MAbQC6TAN5NaaJiX0BmfSiYBfTjVoBOABTDR4gHQAxRVEEYA8lAAiMABmvDgANhh2AJSRVgy0SCwkTACyAJ4AoggYMHiCihC51FrUTKUsRizxTAAqTO6eGAA81QB8dhgAFoqClUbNLUw5OAC2MFC8wKEwkSVlxVRliyxkdnwCQ6PjGNBSg3gjYxNTzgDiMBjp+5u821BRMwtLpfNPT4qBdgCEG4e3zskQbgwAByMCy91mryWnSgEAUsgUAEk8HxQooACbeAAOv3yeEy4ixGDxdgARABlGAARxwOXETEgKl4yh6eAggymoxUglJMS4jyhpSIAHY9gcttBnABhUSwFRWKHmSEWZXKogVVgJaoAfjqHi8vgCwTCTVa7S6PVYfVaYuuk2myper0Yq34tt+O2kP3G9tO50u4pu0AhAqWTsFSg+3yuHqg/0BILBEWiyojIqY6KCIXCsVDUPT3qDcZlUDlGAVryVoarizVGqqtXJygA5lN+ubur1+u6fVMHotw0sXWse0XdoXfWcLjGJXcU3m5qnXu8voW/gCgaDwfOI4sYXCZPImMjURjsbiCgSYESSRTqbS8PTGRhmbkZOyYJychgeXyqEunhHQJ9QwccZyLaVZW/XNd0jOw10lDdE23ftYKYfd4SPE9eDRTEcS2PErxvAoyUpGk6QkZ9Xx6YZoAkTpeDwJgCgkT8YC5DBeRg3d02AhoKyeGsyiE0o60qLVdSbPBWxgQ0sxNdtOk7K1uwnPtHQA5ZXXWcDbjAwNJ39XTgx3V5ByhFdo0DdcEy3ZNUNg9NM2NHN+VgoCQP064/hLMtuIjFcELjJC7JDND0I6WFMKRFEcLPfCbkIhBCWJEi73Ix9KIKF8WSYWjYAixjmNkDl2O/Li3J40U+K8ASlhElUqHMIA)

---

# Single/SingleOrDefault

```csharp {all|5|10}
public static T Single<T>(this IEnumerable<T> enumerable)
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        if(!enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains no elements");

        var first = enumerator.Current;

        if(enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains more than one element");

        return first;
    }
}

public static T? SingleOrDefault<T>(this IEnumerable<T> enumerable)
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        if(!enumerator.MoveNext())
            return default;

        var first = enumerator.Current;

        if(enumerator.MoveNext())
            throw new InvalidOperationException("Sequence contains more than one element");

        return first;
    }
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaUBua6gNwEMomAZwg4oYGEwC8TPDADuTAJZ4MAbQC6TAN5NaTAL6catAJwAKYaPEA6AMrKA5gBsYAeSgARGADNeOJxhmAJRBRgy0SCwkTACyAJ4AoggYMHiCihBp1FrUTHksAMwsEUwAKkwAYopQghgAPKUAfGYYABaKgsUFDY1MqTgAtjBQvMAuQbn5OVT5syxkZnwC/UMjGNBSfXiDw6Mu1gDiMBgJ26u861DBEzNzedN3d4reZgCEK7uX1jEQ3DAAcjBktdJo85m0oBAFLIFABJPB8JyKAAmrgADp8MngkuI0RgsWYAES2GAARxwqXETEgKl4yk6eAgfRcQxUgkJoS4tzBeSIAHYtjs1tBrABhUSwFRGMH6UEGOVyohFViRUoAfkq1Vq7i8vn89SaLXanVY3SagvOYxgN1mD0ejEW/Atnw20g+Iyth2OpyFF2gIO5cztPKUL3eZxdUG+vwBQMCITlIf5TGRPj8ATCgbByfdfqj4qgkow0sessDZdmiuVJXK9jwzhgPSNHS6PWdHvGcuDcwdS3bec2uc9RxOEeFVwTWamicezzeua+Pz+gOBk5DswhUJk8iY8MRKPRmMyOJgeIJxLJFLwVJpGDpaRkTJgLNSGHZnKoM7ufe8Wowg7HPMxQlV9M3XUMzAXEUl1jVcbXAphN2hHc914JFUQxNYsRPM9MiJElyUpCRb3vToBmgCQ2l4PAmEyCRnxgVkMA5MD12TX8amLL95XLBVAyVYpVQ1OsGx1NN9SbNoW1NNsh07QNu1mXsnSggQ3UA4dvUAy4AzBRTZzDVTo2XONdIQ5NUz1DMuXAn8/wA30vgLItWJDOcjJgld43g8CkO3OEETQg9MIubCEFxfE8IvQjr2IzI73pJhyNgRDWmo2jZGZRjXxYmy2IFDjahLO4K3yMt9CAA=)

---

# FirstOrDefault/SingleOrDefault (with predicate)

```csharp
source
    .Where(person => person.Age > 18)
    .First();
```

is equivalent to

```csharp
source
    .First(person => person.Age > 18);
```

---

# FirstOrDefault (with predicate)

```csharp
public static T? FirstOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        while(enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (predicate(current))
                return current;
        }
        return default;
    }
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaUBua6gNwEMomAZwg4oYGEwC8TPDADuTAAowowvAG0AukwDeTak0Mz5SlWoAUAOV4BbGCCYAiAFIQAFnkdomAQQDm9ky0AMwAlGgGRrIKyqoQeFa2gY4AsvwAnl6+AQ7k4ZGG0aZxCdZ2Di5Jgln+gbQAHPlURsYxZvGJ5U4AogA2AJYAXrzAMBhuNTlBAKxNLUWxFmXJAOr9vQO2k4GzEc1RJosdyxUAyvy8E961DrRkTQC+nDS0AJzmwqLiAHQAYv2qDAAeSgABEYAAzXg4XoYcwAB3aeCkAD4mIiSt9akw0Q1QqFngxaEgWCQmCl0t0EBgYHhBP14oJqDoCixgixiUwACoAfiY/0BIPBUJhGAAPFyUeZxv1BBzghK0bScHYoCNejBvERphLvMAIBBemj4bAACb9MC8GmhVks/YtQyMcx8ATK1VW6BSJhulTqmDfADiY26eBVvow0HM+NZLTtDvjTDkbnWMHMPrVEag3xSEG4MEsMGpUZt9oThjjZYTLqYYFEsDwGC96Y9WYAwnXaRhnpWE/0IUwEWaLVbU7WoPWMNHSz340QAOw1jsN7szh4xhPzpimyHQ2Er+Nr0uH1lEdmsEm8pinfp4PwaoU70WK6XJuWsBWS72h93ADVanVcnqBpGuiQ6Wtatrro6ZDOvwX5hhmnrSM2v7+kGGAhghLbFlBui4S0SYpmm37htA2a5vmhZwlOM54dOM7VmOE5NiRiFtkuXb4b2/aDjA5rgaOHE0bRRgViJDoAPQSUwEIiHgpoyQCggYN4ADWMAwPCTB+BAN5+Ew+AYOsX4KZ6EI3gpggwJA8lcT2hEagOzaZuReYFkWwnibGdkiX2A4mnxw40sRWEue246dp5Xk9uMUAQAoRQAJJ4HwAymkCGJWgyeBUuI8JGR0jinDAACOOC0uINbxBgvA3nKNjQBI4y8Mi8QSDAGp2A2jgEj5ZaHtFLSbkxnb7iJA2rj5m7biKe64RNhiHseNDsrAvCmvEvTpEwsCQFAlkYLgYCNkcyLmKwzDLN4N6NrUBJAA=)

---

# SingleOrDefault (with predicate)

```csharp
public static T? SingleOrDefault<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
{
    using(var enumerator = enumerable.GetEnumerator())
    {
        while(enumerator.MoveNext())
        {
            var current = enumerator.Current;
            if (predicate(current))
            {
                // found first, keep going until end or find second
                while (enumerator.MoveNext())
                {
                    if (predicate(enumerator.Current))
                        throw new InvalidOperationException("Sequence contains more than one element");
                }
                return current;
            }
        }
        return default;
    }
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaUBua6gNwEMomAZwg4oYGEwC8TPDADuTAAowowvAG0AukwDeTak0Mz5SlWoAUAOV4BbGCCYAiAFIQAFnkdomAQQDm9ky0AMwAlGgGRrIKyqoQeFa2gY4AsvwAnl6+AQ7k4ZGG0aZxCdZ2Di5Jgln+gbQAHPlURsYxZvGJ5U4AogA2AJYAXrzAMBhuNTlBAKxNLUWxFmXJAOr9vQO2k4GzEc1RJosdyxUAyvy8E961DrRkTQC+nDS0AJzmwqLiAHSn/Xh+XowADyUAAIjAAGa8HC9DDmAAO7TwUgAfEwkSVvrUmOiGqFQs8GLQkCwSEwUuluggMDA8IJ+vFBNQdAUWMEWCSmAAVAD8TAAYv1VBhQRDobCMAAebmo8zjfqCTnBGXouk4OxQEZA7xEaYy7zACAQXrohGwAAm/TAvFpoTZrP2LUMjHMfAE6s1tugUiYnpU2pg3wA4mNungNQGMNBzAS2S1Hc6k0w5G51jBzP6tdGoN8UhBuDBLDAabH7U7k4ZE5Xk+6mGBRLA8BhfVnvbmAMKNukYZ415P9SFMRGW622jMNqBNjBxiv9pNEADs9e7zb784e8eTS6YFqhMLh66Tm4rJ7ZRA5rFJfKYfwBQLF+8lqvlaaVrBVsr9Ea9wB1LH1blDWNU0MVHG07QdLcXTIN1+G/SNsx9aQ2z/INQwwcNEPbMtoN0PCWlTdNMx/KNoDzAsixLeFZ3nfC53nOtJ2nVtSKQztV17AiByHEcYCtCCJ042i6KMatROdAB6SSmEhEQ8AtWThUEDBvAAaxgGAESYPwIH+PwmHwDB1m/RSfUhf5FMEGBIAU7j+yIoFhzbHMKMLYtSxEiSE3s0TB2Hc1+LHWkSOw1yuynHsvO8/txigCAFCKABJPA+AGC1gUxW1GTwalxARYyOkcU4YAARxwOlxHreIMF4f4lRsaAJHGXgUXiCQYCBOxm0cQlfMrE8YpaHdmJ7I9RMGjdfJ3PcJUPPDJsME8zxoDlYF4C14l6dImFgSAoCsjBcDAFsjhRcxWGYZZvH+FtakJIA)

---
layout: statement
---

## Single() should only be used for testing!

---
layout: center 
---

# ToList() and ToArray()

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
    .ForEach(item => Console.WriteLine(item));
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

---

