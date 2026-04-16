namespace MoAF.Core.Modules
{
    public interface IModuleInfo
    {
        string ModuleName { get; set; }
        Type ModuleType { get; set; }
        ModuleState ModuleState { get; set; }
    }
}
