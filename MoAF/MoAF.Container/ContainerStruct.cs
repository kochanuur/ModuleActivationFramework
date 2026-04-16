namespace MoAF.Container
{
    /// <summary>
    /// Singletonの時は、登録時にインスタンス化する
    /// Transientの時は、取得時にインスタンス化する
    /// </summary>
    internal enum LifeSpan
    {
        Singleton,
        Transient,
        GetOnly
    }

    internal enum InstanceState
    {
        NotCreate,
        Created,
        GetOnly
    }

    internal class ContainerStruct
    {
        /// <summary>
        /// インスタンスのライフスパン
        /// </summary>
        internal LifeSpan LifeSpan { get; set; }

        /// <summary>
        /// 登録に使用されたクラス（継承含む）
        /// </summary>
        internal Type ServiceType { get; set; }

        /// <summary>
        /// 実際に登録されたクラス（継承含まない）
        /// </summary>
        internal Type ImplementType { get; set; }

        /// <summary>
        /// 登録に使用したキー
        /// </summary>
        internal string Key { get; set; }

        /// <summary>
        /// 作成されたインスタンス
        /// </summary>
        internal object Instance { get; set; }

        /// <summary>
        /// インスタンスの作成状態
        /// </summary>
        internal InstanceState State { get; set; }
    }
}
