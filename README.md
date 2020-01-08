# Decor.NET

### What is it?
Sometimes we need to write some code that affects the whole code base, like logging. But adding this unrelated code to each method is messy. Instead we can write this code in one place and apply it to any method we want dynamically.

Decor makes it easy to add additional behaviour to existing methods using the C# attributes and without modifying the target method's body.

### How to get started?
Create a decorator for cross-cutting code like logging.
```csharp
public class LoggingDecorator : IDecorator
{
    public void OnBefore(CallInfo callInfo)
    {
        // Do the logging before method is called here...
    }

    public void OnAfter(CallInfo callInfo)
    {
        // And after the method here...
    }
}
```
Apply a decorator to appropriate methods using the attribute `[Decorate(typeof(...))]`.
```csharp
public class SomeClass
{
    [Decorate(typeof(LoggingDecorator))]
    public void SomeMethod() 
    {
        // The code inside the decorated method is left unchanged. 
    }
}
```
Add Decor, the created decorator and the decorated class to dependency container.
```csharp
services.AddDecor()
    .AddTransient<LoggingDecorator>()
    .AddTransientDecorated<SomeClass>();
```
And that's it! Each time the method with an attribute is invoked—the respective decorator will be invoked as well.
