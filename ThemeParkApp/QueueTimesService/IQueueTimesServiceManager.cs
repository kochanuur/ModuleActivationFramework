namespace QueueTimesService
{
    public interface IQueueTimesServiceManager
    {
        Task<int> LoadAllParkIds();

        Task<int> GetParkId(string parkName);

        Task<Dictionary<string, int>> GetWaitTimeList(int themeParkId);
    }
}
