using MoAF.Abstractions.Container;

namespace MoAF.Container
{
    internal class ContainerRegistry : IContainerRegistry
    {
        internal Action<ContainerStruct>? SetContainerStructHandler;
        internal Action<ContainerStruct>? RemoveContainerStructHandler;

        /// <summary>
        /// 継承元で登録したい場合
        /// </summary>
        /// <typeparam name="TFrom"></typeparam>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="name"></param>
        public void RegisterSingleton<TFrom, TTo>(string name)
            where TFrom : class
            where TTo : class, TFrom
        {
            SetContainerStructHandler!(new ContainerStruct()
            {
                LifeSpan = LifeSpan.Singleton,
                ServiceType = typeof(TFrom),
                ImplementType = typeof(TTo),
                Key = name,
                Instance = null,
                State = InstanceState.NotCreate
            });
        }

        /// <summary>
        /// 実装クラスで登録する場合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void RegisterSingleton<T>()
            where T : class
        {
            SetContainerStructHandler!(new ContainerStruct()
            {
                LifeSpan = LifeSpan.Singleton,
                ServiceType = typeof(T),
                ImplementType = typeof(T),
                Key = string.Empty,
                Instance = null,
                State = InstanceState.NotCreate
            });
        }

        public void RegisterSingleton<T>(System.Func<System.IServiceProvider, T> implementationFactory)
            where T : class
        {
        }

        public void RegisterSingleton(System.Type type)
        {
            SetContainerStructHandler!(new ContainerStruct()
            {
                LifeSpan = LifeSpan.Singleton,
                ServiceType = type,
                ImplementType = type,
                Key = string.Empty,
                Instance = null,
                State = InstanceState.NotCreate
            });
        }

        public void RemoveSingleton(System.Type type)
        {
            RemoveContainerStructHandler!(new ContainerStruct()
            {
                LifeSpan = LifeSpan.Singleton,
                ServiceType = type,
                ImplementType = type,
                Key = string.Empty,
                Instance = null,
                State = InstanceState.Created
            });
        }
    }
}
