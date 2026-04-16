using Common.Interfaces;
using Common.Types;
using MoAF.Abstractions.Container;
using MoAF.Core.Modules;

namespace AppMain.Manager
{
    internal class AppModuleManager
    {
        private readonly IContainerProvider container;
        private readonly List<IModuleCommon> modules;
        private readonly List<IService> serviceModules;
        private readonly List<IThemePark> themeParkModules;
        private readonly ServiceModuleManager serviceModuleManager;
        private readonly ThemeParkModuleManager themeParkModuleManager;

        public AppModuleManager(IContainerProvider container, ILoadModuleCompletedHandler moduleHandler)
        {
            this.container = container;

            moduleHandler.LoadModuleCompleted += AppModuleManager_LoadModuleCompleted;
            moduleHandler.LoadAllModuleCompleted += AppModuleManager_LoadAllModuleCompleted;

            modules = new List<IModuleCommon>();
            serviceModules = new List<IService>();
            themeParkModules = new List<IThemePark>();
            serviceModuleManager = new ServiceModuleManager();
            themeParkModuleManager = new ThemeParkModuleManager();
        }

        public Task<int> LoadAllParkIds()
        {
            return serviceModuleManager.LoadAllParkIds(serviceModules);
        }

        public List<string> GetThemeParkName()
        {
            return themeParkModuleManager.GetThemeParkName(themeParkModules);
        }

        public async Task<Dictionary<string, int>> GetWaitTimeList(int themeParkNum)
        {
            return await themeParkModuleManager.GetWaitTimeList(themeParkModules[themeParkNum]);
        }

        private void AppModuleManager_LoadModuleCompleted(object? sender, LoadModuleCompletedEventArgs e)
        {
            Type? type = e.ModuleInfo?.ModuleType;

            if (type != null)
            {
                IModuleCommon? module = container.Resolve(type) as IModuleCommon;
                if (module != null)
                {
                    modules.Add(module);
                }
            }
        }

        private void AppModuleManager_LoadAllModuleCompleted(object? sender, LoadAllModuleCompletedEventArgs e)
        {
            if (AreLoadAllModuleCompleted(e.ModuleInfos))
            {
                InitializeModules();
                CollectModuleTypes();
            }
        }

        private bool AreLoadAllModuleCompleted(List<IModuleInfo> infos)
        {
            foreach (IModuleInfo info in infos)
            {
                if (info.ModuleState != ModuleState.Initialized)
                {
                    return false;
                }
            }

            return true;
        }

        private void InitializeModules()
        {
            foreach (IModuleCommon module in modules)
            {
                InitializeModule(module);
            }
        }

        private void InitializeModule(IModuleCommon module)
        {
            Dictionary<InterfaceType, IModuleCommon[]> implementedBy = new Dictionary<InterfaceType, IModuleCommon[]>();
            ModuleInfo info = module.GetModuleInfo();

            if (info.InterfaceUsed != null)
            {
                // モジュールが利用する機能I/Fを実装する他モジュールのインスタンスリストを生成する
                foreach (InterfaceType usedInterface in info.InterfaceUsed)
                {
                    List<IModuleCommon> interfaces = modules.Where(x => x.GetModuleInfo().InterfaceImplemented.Any(i => i == usedInterface)).ToList();
                    implementedBy.Add(usedInterface, interfaces.ToArray());
                }
            }

            module.Initialize(implementedBy);
        }

        /// <summary>
        /// モジュールの実装I/Fタイプ毎にリストへ追加する
        /// </summary>
        private void CollectModuleTypes()
        {
            foreach (IModuleCommon module in modules)
            {
                ModuleInfo info = module.GetModuleInfo();
                foreach (InterfaceType type in info.InterfaceImplemented)
                {
                    switch (type)
                    {
                        case InterfaceType.Service:
                            serviceModules.Add((IService)module);
                            break;
                        case InterfaceType.ThemePark:
                            themeParkModules.Add((IThemePark)module);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
