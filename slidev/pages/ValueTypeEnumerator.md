---
layout: section
---

# Value-type Enumerators

---

# For

```csharp {all|2|4}
var array = new int[] { 0, 1, 2, 3 };
for (var index = 0; index < source.Length; index++)
{
    var item = source[index];
    Console.WriteLine(item);
}
```

[SharpLab](https://sharplab.io/#v2:EYLgxg9gTgpgtADwGwBYA0AXEUCuA7AHwAEAmABgFgAoUgRmqLIAIjaA6AGQEs8BHAbmrUAbgEMoTAM4QcUMDCYBeJgFE8OALYwoo4ABsYbAEqi8AcxgAKMmia0yASjYAVCAEEoOgJ6WHgqtQAZtBMlmISPAAmMAhKTGT8TFExTAA8UjJyhhww5hgAFonJCADUJQ7UAN7UTLVM4UkYMBpx0rLyANrFALr+dSy0AJyWXE0aftQAvkA===)

---

# For (generated code)

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

# 'For' requires an indexer!
