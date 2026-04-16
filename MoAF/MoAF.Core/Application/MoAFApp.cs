using MoAF.Abstractions.Container;
using MoAF.Core.Exception;
using MoAF.Core.Modules;
using System.Reflection;

namespace MoAF.Core.Application
{
    public abstract class MoAFApp
    {
        public static Func<IContainerRegistry>? ResolveRegistry { get; set; }

        public static Func<IContainerProvider>? ResolveProvider { get; set; }

        protected IContainerProvider? container;
        private IContainerRegistry? registry;
        private IModuleCatalog? moduleCatalog;
        private IModuleManager? moduleManager;

        protected MoAFApp()
        {
            var asm = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "MoAF.Container.dll"));
            var type = asm.GetType("MoAF.Container.ContainerAutoStarter");
            type!.GetMethod("Initialize")!.Invoke(null, null);

            registry = ResolveRegistry?.Invoke() ?? throw new InvalidOperationException("Registry not set.");
            container = ResolveProvider?.Invoke() ?? throw new InvalidOperationException("Provider not set.");
        }

        public virtual void Run()
        {
            try
            {
                ConfigureModuleCatalog();
                ConfigureModuleManager();
                GetModuleCatalog();
                GetModuleManager();

                RegisterTypes(registry!);

                ModuleCatalogRun();

                ModuleManagerRun();
                RunLoadAllModuleCompleted();

                CreateApp();
            }
            catch (MoAFException ex)
            {
                CatchExceptionLoadingModule(ex.Code, ex.Message);
            }
            catch (System.Exception ex)
            {
                CatchExceptionLoadingModule(MoAFResultCode.SystemException, ex.Message);
            }
        }

        protected abstract ModuleCatalogStruct LoadDirectoryInfo();

        protected virtual void RegisterTypes(IContainerRegistry containerRegistry) { }

        protected abstract Form GetAppForm();

        protected virtual void CatchExceptionLoadingModule(MoAFResultCode code, string message)
        {
            throw new MoAFException(MoAFResultCode.NotOverrideRequiredFunctions, "MoAFApp::CatchExceptionLoadingModule が override されていません。");
        }

        #region// Privateメソッド
        private void ConfigureModuleCatalog()
        {
            registry!.RegisterSingleton<IModuleCatalog, DirectoryModuleCatalog>(nameof(DirectoryModuleCatalog));
        }
        private void ConfigureModuleManager()
        {
            //ContainerManager.Instance.Registry.RegisterSingleton<IModuleManager, ModuleManager>(nameof(ModuleManager));
            // ILoadModuleCompletedHandler で登録しておかないと、ユーザーの方で ILoadModuleCompletedHandler へハンドラ登録ができなくなる
            registry!.RegisterSingleton<ILoadModuleCompletedHandler, ModuleManager>(nameof(ModuleManager));
        }
        private void GetModuleCatalog()
        {
            moduleCatalog = container!.Resolve<IModuleCatalog>(nameof(DirectoryModuleCatalog));
            moduleCatalog!.ModuleCatalogStruct = LoadDirectoryInfo();
        }
        private void GetModuleManager()
        {
            moduleManager = container!.Resolve<ILoadModuleCompletedHandler>(nameof(ModuleManager)) as IModuleManager;
        }
        private void ModuleCatalogRun()
        {
            moduleCatalog!.Run();
        }
        private void ModuleManagerRun()
        {
            moduleManager!.SetModuleCatalog(moduleCatalog!);
            moduleManager.Run();
        }
        private void RunLoadAllModuleCompleted()
        {
            moduleManager!.RunLoadAllModuleCompleted();
        }
        private void CreateApp()
        {
            System.Windows.Forms.Application.Run(GetAppForm());
        }
        #endregion
    }
}
