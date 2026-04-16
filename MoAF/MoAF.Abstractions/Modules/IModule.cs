using MoAF.Abstractions.Container;

namespace MoAF.Abstractions.Modules
{
    public interface IModule
    {
        void RegisterTypes(IContainerRegistry containerRegistry);

        void OnInitialized(IContainerProvider containerProvider);
    }
}
