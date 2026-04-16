using Common.Interfaces;
using Common.Types;
using MoAF.Abstractions.Container;
using MoAF.Abstractions.Modules;

namespace TokyoDisneyLand
{
    public class TokyoDisneyLandPortal : IModule, IModuleCommon, IThemePark
    {
        private const string THEME_PARK_NAME_DISPLAY = "東京ディズニーランド";

        ITokyoDisneyLandManager? manager;
        private List<IService> serviceModuleList;

        public TokyoDisneyLandPortal()
        {
            serviceModuleList = new List<IService>();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleCommon, TokyoDisneyLandPortal>(nameof(TokyoDisneyLandPortal));
            containerRegistry.RegisterSingleton<ITokyoDisneyLandManager, TokyoDisneyLandManager>(nameof(TokyoDisneyLandManager));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            manager = containerProvider.Resolve<ITokyoDisneyLandManager>(nameof(TokyoDisneyLandManager));
        }

        public ModuleInfo GetModuleInfo()
        {
            return new ModuleInfo()
            {
                ModuleName = nameof(TokyoDisneyLandPortal),
                ModuleId = "634AC244-F9A3-4EA5-9260-2167BC3A6FF0",
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
