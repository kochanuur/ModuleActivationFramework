using Common.Interfaces;

namespace TokyoDisneySea
{
    internal interface ITokyoDisneySeaManager
    {
        public void SetServiceModule(List<IService> moduleList);

        public Task<Dictionary<string, int>> GetWaitTimeList();
    }
}
