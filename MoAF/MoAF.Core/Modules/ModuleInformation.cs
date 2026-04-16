namespace MoAF.Core.Modules
{
    internal class ModuleInformation : IModuleInfo
    {
        public string ModuleName { get; set; }
        public Type ModuleType { get; set; }
        public ModuleState ModuleState { get; set; }

    }
}
