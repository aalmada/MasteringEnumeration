---
layout: section
---

# Linq

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