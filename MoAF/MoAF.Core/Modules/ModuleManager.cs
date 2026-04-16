using MoAF.Abstractions.Container;
using MoAF.Abstractions.Modules;

namespace MoAF.Core.Modules
{
    internal class ModuleManager : IModuleManager, ILoadModuleCompletedHandler
    {
        public event EventHandler<LoadModuleCompletedEventArgs>? LoadModuleCompleted;
        public event EventHandler<LoadAllModuleCompletedEventArgs>? LoadAllModuleCompleted;

        private readonly IContainerRegistry registry;
        private readonly IContainerProvider provider;

        private List<IModuleInfo>? modules;

        public ModuleManager(IContainerRegistry registry, IContainerProvider provider)
        {
            this.registry = registry;
            this.provider = provider;
        }

        public void SetModuleCatalog(IModuleCatalog moduleCatalog)
        {
            modules = moduleCatalog.Modules != null ? moduleCatalog.Modules : new List<IModuleInfo>();
        }

        public void Run()
        {
            foreach (IModuleInfo info in modules!)
            {
                LoadModule(info);
                Configure(info);
                Initialize(info);
                InitializeCompleted(info);
            }
        }

        public void RunLoadAllModuleCompleted()
        {
            bool isImplemented = LoadAllModuleCompleted != null;

            if (!modules!.Any(x => x.ModuleState != ModuleState.Initialized) && isImplemented)
            {
                if (LoadAllModuleCompleted != null)
                {
                    LoadAllModuleCompleted(this, new LoadAllModuleCompletedEventArgs(modules!));
                }
                else
                {
                    // 例外
                }
            }
        }

        #region// Privateメソッド
        private void LoadModule(IModuleInfo info)
        {
            if (info.ModuleState == ModuleState.NotStarted)
            {
                info.ModuleState = ModuleState.LoadingTypes;
                registry.RegisterSingleton(info.ModuleType);
                info.ModuleState = ModuleState.ReadyForConfiguration;
            }
        }

        private void Configure(IModuleInfo info)
        {
            if (info.ModuleState == ModuleState.ReadyForConfiguration)
            {
                IModule? module = provider.Resolve(info.ModuleType) as IModule;
                if (module != null)
                {
                    info.ModuleState = ModuleState.Configurating;
                    module.RegisterTypes(registry);
                    info.ModuleState = ModuleState.Configurated;
                }
            }
        }

        private void Initialize(IModuleInfo info)
        {
            if (info.ModuleState == ModuleState.Configurated)
            {
                IModule? module = provider.Resolve(info.ModuleType) as IModule;
                if (module != null)
                {
                    info.ModuleState = ModuleState.Initializing;
                    module.OnInitialized(provider);
                    // RegisterTypesとOnInitializedを呼ぶためにインスタンス化したインスタンスを消す必要はない？
                    //containerManager.Registry.RemoveSingleton(info.ModuleType);
                }
            }
        }

        private void InitializeCompleted(IModuleInfo info)
        {
            bool isImplemented = LoadModuleCompleted != null;

            if (info.ModuleState == ModuleState.Initializing && isImplemented)
            {
                info.ModuleState = ModuleState.Initialized;
                LoadModuleCompleted(this, new LoadModuleCompletedEventArgs(info));
            }
            else if (isImplemented)
            {
                LoadModuleCompleted(this, new LoadModuleCompletedEventArgs(null));
            }
            else
            {
                //throw new NotImplementedException();
            }
        }
        #endregion
    }
}
