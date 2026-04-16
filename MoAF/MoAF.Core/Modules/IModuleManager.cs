namespace MoAF.Core.Modules
{
    public class LoadModuleCompletedEventArgs : EventArgs
    {
        public IModuleInfo? ModuleInfo { get; private set; }

        public LoadModuleCompletedEventArgs(IModuleInfo? moduleInfo)
        {
            ModuleInfo = moduleInfo;
        }
    }
    public class LoadAllModuleCompletedEventArgs : EventArgs
    {
        public List<IModuleInfo> ModuleInfos { get; set; }

        public LoadAllModuleCompletedEventArgs(List<IModuleInfo> moduleInfos)
        {
            ModuleInfos = moduleInfos;
        }
    }

    public interface ILoadModuleCompletedHandler
    {
        event EventHandler<LoadModuleCompletedEventArgs> LoadModuleCompleted;
        event EventHandler<LoadAllModuleCompletedEventArgs> LoadAllModuleCompleted;
    }

    internal interface IModuleManager
    {
        void SetModuleCatalog(IModuleCatalog moduleCatalog);
        void Run();
        void RunLoadAllModuleCompleted();
    }
}
