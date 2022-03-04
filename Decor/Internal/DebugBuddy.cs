namespace Decor.Internal
{
    internal static class DebugBuddy
    {
        public static void WaitForDebugger()
        {
            while (!System.Diagnostics.Debugger.IsAttached)
                System.Threading.Thread.Sleep(500);
        }
    }
}
