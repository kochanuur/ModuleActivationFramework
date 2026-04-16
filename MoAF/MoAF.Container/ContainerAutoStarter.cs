using System.Runtime.CompilerServices;

namespace MoAF.Container
{
    internal static class ContainerAutoStarter
    {
        [ModuleInitializer]
        public static void Initialize()
        {
            _ = ContainerManager.Instance;
        }
    }
}
