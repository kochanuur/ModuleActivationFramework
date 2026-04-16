using Common.Interfaces;


namespace UniversalStudioJapan
{
    internal interface IUniversalStudioJapanManager
    {
        public void SetServiceModule(List<IService> moduleList);

        public Task<Dictionary<string, int>> GetWaitTimeList();
    }
}
