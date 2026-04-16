namespace MoAF.Abstractions.Modules
{

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public sealed class ModuleConstructorAttribute : System.Attribute
    {
        /// <summary>
        /// true の場合、この ctor が解決できないならフォールバックせずエラー。
        /// 現実装では、false が来ても無視
        /// </summary>
        public bool Required { get; set; } = true;
    }
}
