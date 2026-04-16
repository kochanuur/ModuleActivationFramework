namespace Common.Types
{
    public enum InterfaceType
    {
        Service,
        ThemePark,
    }

    public struct ModuleInfo
    {
        public string ModuleName;
        public string ModuleId;
        public InterfaceType[] InterfaceImplemented;
        public InterfaceType[] InterfaceUsed;
    }

}
