namespace Decor.UnitTests.Data
{
    public class DecoratorWithDependencies : IDecorator
    {
        public int CallCount { get; set; }
        public SomeDependency SomeDependency { get; }

        public DecoratorWithDependencies(SomeDependency someDependency)
        {
            SomeDependency = someDependency;
        }

        public void OnAfter(CallInfo callInfo) => CallCount++;

        public void OnBefore(CallInfo callInfo) => CallCount++;
    }
}
