using Common.Interfaces;

namespace AppMain.Manager
{
    public class ThemeParkModuleManager
    {
        public List<string> GetThemeParkName(List<IThemePark> moduleList)
        {
            List<string> themeParkNameList = new List<string>();
            foreach (IThemePark module in moduleList)
            {
                themeParkNameList.Add(module.GetThemeParkName());
            }
            return themeParkNameList;
        }

        public async Task<Dictionary<string, int>> GetWaitTimeList(IThemePark module)
        {
            return await module.GetWaitTimeList();
        }
    }
}
