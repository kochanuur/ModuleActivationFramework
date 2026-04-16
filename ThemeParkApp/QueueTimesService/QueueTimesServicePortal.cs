using Common.Interfaces;
using Common.Types;
using MoAF.Abstractions.Container;
using MoAF.Abstractions.Modules;

namespace QueueTimesService
{
    public class QueueTimesServicePortal : IModule, IModuleCommon, IService
    {
        private IQueueTimesServiceManager? manager;

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IModuleCommon, QueueTimesServicePortal>(nameof(QueueTimesServicePortal));
            containerRegistry.RegisterSingleton<IQueueTimesServiceManager, QueueTimesServiceManager>(nameof(QueueTimesServiceManager));
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // ここ、登録時にキーを登録していたら、キーを指定して探索しないと例外がでるようになっている
            // 要修正
            manager = containerProvider.Resolve<IQueueTimesServiceManager>(nameof(QueueTimesServiceManager));
            //manager = containerProvider.Resolve<QueueTimesServiceManager>();
        }

        public ModuleInfo GetModuleInfo()
        {
            return new ModuleInfo()
            {
                ModuleName = nameof(QueueTimesServicePortal),
                ModuleId = "05D78061-2FAF-4CDF-BE24-B994C37BA8BD",
                InterfaceImplemented = new InterfaceType[] { InterfaceType.Service },
                InterfaceUsed = new InterfaceType[] { }
            };
        }

        public void Initialize(Dictionary<InterfaceType, IModuleCommon[]> interfaces)
        {

        }

        public async Task<int> LoadAllParkIds()
        {
            return await manager!.LoadAllParkIds();
        }

        public async Task<int> GetParkId(string parkName)
        {
            return await manager!.GetParkId(parkName);
        }

        public async Task<Dictionary<string, int>> GetWaitTimeList(int themeParkId)
        {
            return await manager!.GetWaitTimeList(themeParkId);
        }
    }
}
