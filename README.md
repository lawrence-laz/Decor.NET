# Decor.NET
[![NuGet Version](https://img.shields.io/nuget/v/Decor.svg)](https://www.nuget.org/packages/Decor "NuGet Version")
[![NuGet Downloads](https://img.shields.io/nuget/dt/Decor)](https://www.nuget.org/packages/Decor "NuGet Downloads")
[![GitHub Action Status](https://github.com/lawrence-laz/Decor.NET/workflows/continuous%20integration/badge.svg)](https://github.com/lawrence-laz/Decor.NET/actions?query=workflow%3A%22continuous+integration%22)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/16ff5fdad18d41879814228d78e754d1)](https://www.codacy.com/manual/lawrence-laz/Decor.NET?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=lawrence-laz/Decor.NET&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://api.codacy.com/project/badge/Coverage/16ff5fdad18d41879814228d78e754d1)](https://www.codacy.com/manual/lawrence-laz/Decor.NET?utm_source=github.com&utm_medium=referral&utm_content=lawrence-laz/Decor.NET&utm_campaign=Badge_Coverage)
## What is it?
This package provides a nice and simple way to execute any code *before* and *after* any other method. This is particularly useful for things like: logging, profiling, retry logic, caching, etc.

```csharp
[Decorate(typeof(LoggingDecorator))]
void DoWork() 
{
    Console.WriteLine("Working...");
}
```

```csharp
public class LoggingDecorator : IDecorator
{    
    public async Task OnInvoke(Call call)
    {
        Console.WriteLine("Will do some work!");
        await call.Next();
        Console.WriteLine("Work is finished!");
    }
}
```

```text
Output is:
    Will do some work!
    Working...
    Work is finished!
```

## How to get started?
There are two ways to use this library:
1. [With Microsoft.DependencyInjection](#with-microsoft-dependency-injection)
2. [Without dependency injection](#without-dependency-injection)

### With Microsoft dependency injection
1.  Install the main package and Microsoft DependencyInjection integration:
    ```powershell
    PS> Install-Package Decor
    PS> Install-Package Decor.Extensions.Microsoft.DependencyInjection
    ```

2.  Create a decorator implementing `IDecorator` interface.

3.  Add `[Decorate(typeof(YourDecorator))]` attributes to methods to be decorated.

4.  Register to dependency container:
    ```csharp
    services.AddDecor()
        .AddTransient<YourDecorator>() // Transient means decorator will inherit target's lifetime.
        .AddScoped<SomeService>().Decorated(); 
    ```

### Without dependency injection
1.  Install the main package:
    ```powershell
    PS> Install-Package Decor
    ```

2.  Create a decorator implementing `IDecorator` interface.

3.  Add `[Decorate(typeof(YourDecorator))]` attributes to methods to be decorated.

4.  Create decorated objects using `Decorator` class:
    ```csharp
    var service = new SomeService();
    var decoratedService = new Decorator().For(service);
    ```
