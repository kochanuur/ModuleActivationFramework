using Common.Interfaces;

namespace TokyoDisneyLand
{
    internal interface ITokyoDisneyLandManager
    {
        public void SetServiceModule(List<IService> moduleList);

        public Task<Dictionary<string, int>> GetWaitTimeList();
    }
}
