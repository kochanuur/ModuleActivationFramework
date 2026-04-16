namespace MoAF.Abstractions.Container
{
    public interface IContainerRegistry
    {
        // パターンとしてあるもの
        // 1. 継承系を設定してシングルトンでインスタンス登録
        // 2. 継承系を設定しないでシングルトンでインスタンス登録
        // 3. 継承系を設定してインスタンス登録(複数可)
        // 4. 継承系を設定しないでインスタンス登録(複数可)

        void RegisterSingleton<TFrom, TTo>(string name) 
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterSingleton<T>()
            where T : class;

        void RegisterSingleton<T>(System.Func<System.IServiceProvider, T> implementationFactory)
            where T : class;

        void RegisterSingleton(System.Type type);

        void RemoveSingleton(System.Type type);
    }
}
