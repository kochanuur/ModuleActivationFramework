namespace MoAF.Abstractions.Modules
{
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class ArgumentNamedAttribute : System.Attribute
    {

        public string Name { get; }
        public ArgumentNamedAttribute(string name) => Name = name;

    }
}
