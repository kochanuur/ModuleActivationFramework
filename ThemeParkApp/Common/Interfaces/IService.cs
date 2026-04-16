namespace Common.Interfaces
{
    public interface IService
    {
        public Task<int> LoadAllParkIds();

        // これは IThemePark から呼び出す
        public Task<int> GetParkId(string parkName);

        // これは IThemePark から呼び出す
        public Task<Dictionary<string, int>> GetWaitTimeList(int themeParkId);
    }
}
