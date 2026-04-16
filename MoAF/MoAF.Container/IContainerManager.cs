using MoAF.Abstractions.Container;

namespace MoAF.Container
{
    internal interface IContainerManager
    {
        IContainerRegistry Registry { get; }
        IContainerProvider Provider { get; }
    }
}
