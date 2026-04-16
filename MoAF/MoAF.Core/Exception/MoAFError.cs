namespace MoAF.Core.Exception
{
    public enum MoAFResultCode
    {
        OK,
        SystemException = -1,

        NotOverrideRequiredFunctions = 1001,
        NotExistPublicCtor,
        AttributeCtorMultiple,
        NonAttributeCtorMultiple,
    }

    internal static class MoAFError
    {
        private static readonly Dictionary<MoAFResultCode, string> MESSAGE = new Dictionary<MoAFResultCode, string>()
        {
            { MoAFResultCode.OK, "成功しました。" },
            { MoAFResultCode.NotOverrideRequiredFunctions, "Overrideが必要な関数が実装されていません。" },
            { MoAFResultCode.NotExistPublicCtor, "Publicコンストラクタが定義されていないクラスが登録されました。" },
            { MoAFResultCode.AttributeCtorMultiple, "ModuleConstructor属性が複数のコンストラクタに付与されています。" },
            { MoAFResultCode.NonAttributeCtorMultiple, "ModuleConstructor属性が付与されておらず、かつ複数コンストラクタが定義されているクラスが登録されました。" },
        };

        /// <summary>
        /// エラーコードからメッセージへの変換
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        internal static string TranslateCodeToMessage(MoAFResultCode code)
        {
            return MESSAGE[code];
        }

        internal static bool Contains(MoAFResultCode code)
        {
            return MESSAGE.ContainsKey(code);
        }
    }
}
