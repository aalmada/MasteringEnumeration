---
layout: section
---

# ForEach

---

# ForEach

```csharp {all|2|4}
var array = new int[] { 0, 1, 2, 3 };
foreach (var item in source)
{
    Console.WriteLine(item);
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AGQEs8BHAbmrUAbgEMoTAM4QcUMDCYBeJgFE8OALYwoo4ABsYbAEqi8AcxgAKMmia0yASjYAVCAEEoOgJ6WHgqtQAZtAwomAAFkyWYhJcGDAaTDxSMnIwDtQA3tRMuSy0AJyWcQl+1AC+QA)

---

# ForEach (generated code)

```csharp {all|1|2|5}
int num = 0;
while (num < array.Length)
{
    Console.WriteLine(array[num]);
    num++;
}
```

---
layout: statement
---

# 'ForEach' uses duck typing!
Generates at compile time different code depending on the source type.

---

# ForEach

- arrays and spans - uses indexer
- other types - uses enumerators

---

# ForEach (minimum requirements)

- type must provide public parameterless method named `GetEnumerator`
- returned type must provide:
    - public parameterless method named `MoveNext` that return a boolean
    - public readable property named `Current`

---

# ForEach (minimum requirements)

```csharp {all|8|11|22|25}
class MyCollection
{
    readonly int[] source;
    
    public MyCollection(int[] source)
        => this.source = source;
    
    public Enumerator GetEnumerator()
        => new Enumerator(this);
    
    public struct Enumerator
    {
        readonly int[] source;
        int index;
        
        public Enumerator(MyCollection enumerable)
        {
            source = enumerable.source;
            index = -1;
        }
        
        public int Current 
            => source[index];
        
        public bool MoveNext()
            => ++index < source.Length;
    }
}

```
[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmuoDcBDKAAgGcIcowY2AvGzwwA7mwCyATwDCEADbyYYDAEsIeABQjRAbQC6bAN5k0bWmZJmAzGZQBfAJQBuagDNoMZmAAWmluyqGDAAtmyqeJzcvDCO1GwJbES0AJyaQaEuDDQkkrIKSirqeNRG8YmwzAAmGvJS4XgYBlE8fK5UiWzlCUTWeXKKymoa6Y3NXK2x3Z2CAHxsGD6qHAB0EzGCLTHtndO9bACieDghMFDMGNBsAOIwGEcnZxfQmnEdMwkC8zqHx6fnlygmkWyyy7wSez6HAwuBUv0eAOg0zK4JmlRqeDqDSahnWbWmnQiGAaVRgCB2Hy6qM6+we/2eQOkA0Kw0iMD+T2ASjelOMBMpeP4QnZCOYXJga2i+OplIipIQmzgtApH3s/IhMp6fSJbBkPFgjSpvM6Xy2fF0crJ+hVM3VST6wAgCkkEEYMAAcmSMK87Sb5gBqf2WhUAHjNEoAMuyAOaLG1qqgJ6hAA)```

---

# ForEach (minimum requirements generated code)

``` csharp {all|1|2|4}
MyCollection.Enumerator enumerator = new MyCollection(array).GetEnumerator();
while (enumerator.MoveNext())
{
    Console.WriteLine(enumerator.Current);
}
```

---
layout: statement
---

For best performance

# Enumerators should have a value-type!

Reference-types require the use of VTABLES!

---

# ForEach

``` csharp
public static int Sum(MyCollection source)
{
    var sum = 0;
    foreach (var item in source)
        sum += item;
    return sum;
}
```