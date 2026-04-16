namespace MoAF.Core.Modules
{
    public enum ModuleState
    {
        NotStarted,
        LoadingTypes,
        ReadyForConfiguration,
        Configurating,
        Configurated,
        Initializing,
        Initialized
    }
}
