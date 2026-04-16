namespace MoAF.Abstractions.Container
{
    public interface IContainerProvider
    {
        T Resolve<T>(string name)
            where T : class;

        T Resolve<T>()
            where T : class;

        object Resolve(System.Type type);
    }
}
