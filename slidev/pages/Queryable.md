---
layout: section
---

# IQueryable / IAsyncQueryable

---

# IQueryable

```csharp {all|1|8}
public interface IQueryable : IEnumerable
{
  Expression Expression { get; }
  Type ElementType { get; }
  IQueryProvider Provider { get; }
}

public interface IQueryable<out T> : IEnumerable<T>, IQueryable
{
}

public interface IQueryProvider
{
  IQueryable CreateQuery(Expression expression);
  IQueryable<TElement> CreateQuery<TElement>(Expression expression);
  object? Execute(Expression expression);
  TResult Execute<TResult>(Expression expression);
}
```

---

# IAsyncQueryable

```csharp {all|1|8}
public interface IAsyncQueryable
{
  Type ElementType { get; }
  Expression Expression { get; }
  IAsyncQueryProvider Provider { get; }
}

public interface IAsyncQueryable<out T> : IAsyncEnumerable<T>, IAsyncQueryable
{
}

public interface IAsyncQueryProvider
{
  IAsyncQueryable<TElement> CreateQuery<TElement>(Expression expression);
  ValueTask<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken token);
}
```

---

# IQueryable / IAsyncQueryable

- Pull
- Read-only 
- Sequential
- Expression is executed by some engine

---

# IQueryable / IAsyncQueryable

- Entity Framework (Linq-to-SQL)
- LinqToExcel
- LinqToTwitter
- LinqToCsv
- LINQ-to-BigQuery
- LINQ-to-GameObject-for-Unity
- ElasticLINQ
- GpuLinq

---

# Entity Framework

```csharp
using (var db = new BloggingContext())
{
  var blogs = db.Blogs
    .Where(blog => blog.Rating > 3)
    .OrderByDescending(blog => blog.Rating)
    .Take(5)
    .Select(blog => blog.Url);

  Console.WriteLine(blogs.ToSql());
  Console.WriteLine();

  foreach (var blog in blogs)
    Console.WriteLine(blog);
}
```

```sql
SELECT TOP(5) [blog].[Url]
FROM [Blogs] AS [blog]
WHERE [blog].[Rating] > 3
ORDER BY [blog].[Rating] DESC
```

--- 

# AsEnumerable()

```csharp {all|12-14}
using (var db = new BloggingContext())
{
    var blogs = db.Blogs
        .Where(blog => blog.Rating > 3)
        .OrderByDescending(blog => blog.Rating)
        .Take(5)
        .Select(blog => blog.Url);

    Console.WriteLine(blogs.ToSql());
    Console.WriteLine();

    var urls = blogs
        .AsEnumerable()
        .Select(url => $"URL: {url}");

    foreach (var url in urls)
    {
        Console.WriteLine(url);
    }
}
```

---