namespace MoAF.Core.Exception
{
    public class MoAFException : System.Exception
    {
        public MoAFResultCode Code { get; private set; }

        public MoAFException(MoAFResultCode code, string message) : base(FormatMessage(code, message))
        {
            Code = code;
        }

        public MoAFException(MoAFResultCode code, string message, System.Exception innerException) : base(FormatMessage(code, message), innerException)
        {
            Code = code;
        }

        public MoAFException(MoAFResultCode code) : base(GetDefaultMessage(code))
        {
            Code = code;
        }

        private static string FormatMessage(MoAFResultCode code, string message)
        {
            var name = Enum.GetName(typeof(MoAFResultCode), code) ?? "Unknown";
            return $"[{(int)code} {name}] {message}";
        }

        private static string GetDefaultMessage(MoAFResultCode code)
        {
            var name = Enum.GetName(typeof(MoAFResultCode), code) ?? "Unknown";
            string message = MoAFError.Contains(code) ? MoAFError.TranslateCodeToMessage(code) : "Unknown Error.";
            return $"[{(int)code} {name}] {message}";
        }
    }
}
