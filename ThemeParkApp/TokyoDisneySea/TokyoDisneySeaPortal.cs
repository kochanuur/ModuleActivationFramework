using Common.Interfaces;
using Common.Types;
using MoAF.Abstractions.Container;
using MoAF.Abstractions.Modules;

namespace TokyoDisneySea
{
    public class TokyoDisneySeaPortal : IModule, IModuleCommon, IThemePark
    {
        private const string THEME_PARK_NAME_DISPLAY = "東京ディズニーシー";

        ITokyoDisneySeaManager? manager;
        private List<IService> serviceModuleList;

        public TokyoDisneySeaPortal()
        {
            serviceModuleList = new List<IService>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleCommon, TokyoDisneySeaPortal>(nameof(TokyoDisneySeaPortal));
            containerRegistry.RegisterSingleton<ITokyoDisneySeaManager, TokyoDisneySeaManager>(nameof(TokyoDisneySeaManager));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            manager = containerProvider.Resolve<ITokyoDisneySeaManager>(nameof(TokyoDisneySeaManager));
        }

        public ModuleInfo GetModuleInfo()
        {
            return new ModuleInfo()
            {
                ModuleName = nameof(TokyoDisneySeaPortal),
                ModuleId = "8FDC6C66-8434-4B0F-8347-85DA39F289FD",
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
