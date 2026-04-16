namespace MoAF.Core.Modules
{
    public struct ModuleCatalogStruct
    {
        public string ModulePath;
        public string InterfaceDllName;
    }

    internal interface IModuleCatalog
    {
        List<IModuleInfo> Modules { get; }

        ModuleCatalogStruct ModuleCatalogStruct { get; set; }

        void Run();
    }
}
