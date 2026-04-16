using Common.Interfaces;

namespace TokyoDisneyLand
{
    internal class TokyoDisneyLandManager : ITokyoDisneyLandManager
    {
        private const string THEME_PARK_NAME_API = "Walt Disney Attractions:Tokyo Disneyland";

        private List<IService> moduleList;
        private int themeParkId = -1;
        private bool isIdRetrieved = false;

        public TokyoDisneyLandManager()
        {
            moduleList = new List<IService>();
        }

        public void SetServiceModule(List<IService> moduleList)
        {
            this.moduleList = moduleList;
        }

        public async Task<Dictionary<string, int>> GetWaitTimeList()
        {
            if (!isIdRetrieved)
            {
                themeParkId = await GetParkId();
            }

            return await GetWaitTimeListFromModule();
        }

        private async Task<int> GetParkId()
        {
            if (isIdRetrieved)
            {
                return themeParkId;
            }

            foreach (var module in moduleList)
            {
                // サービスモジュールが2つ以上は想定していないため、戻り値をそのまま変数に代入
                themeParkId = await module.GetParkId(THEME_PARK_NAME_API);
            }

            return themeParkId;
        }

        private async Task<Dictionary<string, int>> GetWaitTimeListFromModule()
        {
            var result = new Dictionary<string, int>();
            foreach (var module in moduleList)
            {
                var list = await module.GetWaitTimeList(themeParkId);
                foreach (var item in list)
                {
                    if (!result.ContainsKey(item.Key))
                    {
                        result.Add(item.Key, item.Value);
                    }
                }
            }

            return result;
        }
    }
}
