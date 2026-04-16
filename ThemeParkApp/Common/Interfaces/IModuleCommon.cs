using Common.Types;

namespace Common.Interfaces
{
    public interface IModuleCommon
    {
        /// <summary>
        /// モジュール情報を取得する
        /// </summary>
        /// <returns>モジュール情報</returns>
        ModuleInfo GetModuleInfo();

        /// <summary>
        /// 初期化する
        /// </summary>
        /// <param name="interfaces">モジュールが利用する機能インターフェースを実装する多モジュールのインスタンスリスト</param>
        void Initialize(Dictionary<InterfaceType, IModuleCommon[]> interfaces);
    }
}
