# Decor.NET
[![NuGet Version](https://img.shields.io/nuget/v/Decor.svg)](https://www.nuget.org/packages/Decor "NuGet Version")

### What is it?
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

```
Output is:
    Will do some work!
    Working...
    Work is finished!
```

### How to get started?
There are two ways to use this library:
1. [With Microsoft.DependencyInjection](#with-microsoft-dependency-injection)
2. [Without dependency injection](#without-dependency-injection)

#### With Microsoft dependency injection
1. Install the main package and Microsoft DependencyInjection integration:
```
Install-Package Decor
Install-Package Decor.Extensions.Microsoft.DependencyInjection
```
2. Create a decorator implementing `IDecorator` interface.
3. Add `[Decorate(typeof(YourDecorator))]` attributes to methods to be decorated.
4. Register to dependency container:
```csharp
services.AddDecor()
    .AddTransient<YourDecorator>() // Transient means decorator will inherit target's lifetime.
    .AddScoped<SomeService>().Decorated(); 
```

#### Without dependency injection
1. Install the main package:
```
Install-Package Decor
```
2. Create a decorator implementing `IDecorator` interface.
3. Add `[Decorate(typeof(YourDecorator))]` attributes to methods to be decorated.
4. Create decorated objects using `Decorator` class:
```csharp
var service = new SomeService();
var decoratedService = new Decorator().For(service);
```
