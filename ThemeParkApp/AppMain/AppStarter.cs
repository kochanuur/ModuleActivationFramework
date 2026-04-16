using MoAF.Abstractions.Container;
using MoAF.Core.Application;
using MoAF.Core.Modules;
using AppMain.Manager;

namespace AppMain
{
    internal class AppStarter : MoAFApp
    {
        private string staticModulesDir = $"{Application.StartupPath}\\Modules";
        private string interfaceDllName = "Common.dll";

        protected override ModuleCatalogStruct LoadDirectoryInfo()
        {
            return new ModuleCatalogStruct()
            {
                ModulePath = staticModulesDir,
                InterfaceDllName = interfaceDllName
            };
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<AppModuleManager>();
        }

        protected override Form GetAppForm()
        {
            return new MainWindow(container!);
        }
    }
}
