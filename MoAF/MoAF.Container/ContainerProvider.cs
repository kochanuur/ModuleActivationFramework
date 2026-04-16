using MoAF.Abstractions.Container;

namespace MoAF.Container
{
    internal class ContainerProvider : IContainerProvider
    {
        internal Action<ContainerResolveStruct>? GetContainerStructHandler;

        public T Resolve<T>(string name)
            where T : class
        {
            ContainerResolveStruct containerResolveStruct = new ContainerResolveStruct()
            {
                ResolveType = typeof(T),
                Key = name
            };

            GetContainerStructHandler!(containerResolveStruct);

            return (T)containerResolveStruct.Instance;
        }

        public T Resolve<T>()
            where T : class
        {
            ContainerResolveStruct containerResolveStruct = new ContainerResolveStruct()
            {
                ResolveType = typeof(T),
                Key = string.Empty
            };

            GetContainerStructHandler!(containerResolveStruct);

            return (T)containerResolveStruct.Instance;
        }

        public object Resolve(Type type)
        {
            ContainerResolveStruct containerResolveStruct = new ContainerResolveStruct()
            {
                ResolveType = type,
                Key = string.Empty
            };

            GetContainerStructHandler!(containerResolveStruct);

            return containerResolveStruct.Instance;
        }
    }
}
