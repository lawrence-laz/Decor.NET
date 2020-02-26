# Decor.NET
[![NuGet Version](https://img.shields.io/nuget/v/Decor.svg)](https://www.nuget.org/packages/Decor "NuGet Version")

### What is it?
This package provides a nice and simple way to execute any code *before* and *after* any other method. This is particularly useful for things like: logging, profiling, retry logic, caching, etc.

Basically instead of this:
```csharp
public class Service : IService
{
    ILogger Logger { get; set; }
    
    public void DoWork()
    {
        Logger.Log("Started method DoWork");
        
        // Doing some work here
        
        Logger.Log("Completed method DoWork");
    }
    
    public void DoOtherWork()
    {
        Logger.Log("Started method DoOtherWork");
        
        // Doing some other work here
        
        Logger.Log("Completed method DoOtherWork");
    }
}
```
You can do this:
```csharp
public class Service : IService
{
    [Decorate(typeof(CallLogger))]
    void DoWork() => // Doing some work here
    
    [Decorate(typeof(CallLogger))]
    void DoOtherWork() => // Doing some work here
}

// This can be re-used for any class/method !
public class CallLogger : IDecorator
{
    ILogger Logger { get; set; }
    
    public async Task OnInvoke(Call call)
    {
        Logger.Log("Started method " + call.Method.Name);
        
        await call.Next();
        
        Logger.Log("Completed method " + call.Method.Name);
    }
}
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
    .AddTransient<ProfilerDecorator>()
    .AddTransientDecorated<SomeClass>(); 
    // Notice the '...Decorated' postfix. It is needed for `[Decorate]` attribute to take effect.
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
