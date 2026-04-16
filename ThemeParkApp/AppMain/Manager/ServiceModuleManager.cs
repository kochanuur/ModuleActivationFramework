using Common.Interfaces;

namespace AppMain.Manager
{
    internal class ServiceModuleManager
    {
        public async Task<int> LoadAllParkIds(List<IService> moduleList)
        {
            int result = 0;
            bool isSuccess = true;
            foreach (IService module in moduleList)
            {
                result = await module.LoadAllParkIds();
                if (result != 0)
                {
                    isSuccess = false;
                    break;
                }
            }

            return isSuccess ? 0 : -1;
        }
    }
}
