using Common.Interfaces;
using Common.Types;
using MoAF.Abstractions.Container;
using MoAF.Abstractions.Modules;

namespace UniversalStudioJapan
{
    public class UniversalStudioJapanPortal : IModule, IModuleCommon, IThemePark
    {
        private const string THEME_PARK_NAME_DISPLAY = "ユニバーサルスタジオジャパン";

        IUniversalStudioJapanManager? manager;
        private List<IService> serviceModuleList;

        public UniversalStudioJapanPortal()
        {
            serviceModuleList = new List<IService>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleCommon, UniversalStudioJapanPortal>(nameof(UniversalStudioJapanPortal));
            containerRegistry.RegisterSingleton<IUniversalStudioJapanManager, UniversalStudioJapanManager>(nameof(UniversalStudioJapanManager));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            manager = containerProvider.Resolve<IUniversalStudioJapanManager>(nameof(UniversalStudioJapanManager));
        }

        public ModuleInfo GetModuleInfo()
        {
            return new ModuleInfo()
            {
                ModuleName = nameof(UniversalStudioJapanPortal),
                ModuleId = "F930F2B9-D3AF-4E91-B513-BA3AA946F310",
                InterfaceImplemented = new InterfaceType[] { InterfaceType.ThemePark },
                InterfaceUsed = new InterfaceType[] { InterfaceType.Service }
            };
        }

        public void Initialize(Dictionary<InterfaceType, IModuleCommon[]> interfaces)
        {
            foreach (var interfaceUsed in interfaces)
            {
                if (interfaceUsed.Key == InterfaceType.Service)
                {
                    foreach (var module in interfaceUsed.Value)
                    {
                        var serviceModule = module as IService;
                        if (serviceModule != null)
                        {
                            serviceModuleList.Add(serviceModule);
                        }
                    }
                }
            }

            manager!.SetServiceModule(serviceModuleList);
        }

        public string GetThemeParkName()
        {
            return THEME_PARK_NAME_DISPLAY;
        }

        public async Task<Dictionary<string, int>> GetWaitTimeList()
        {
            return await manager!.GetWaitTimeList();
        }
    }
}
