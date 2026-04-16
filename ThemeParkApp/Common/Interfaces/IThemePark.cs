namespace Common.Interfaces
{
    public interface IThemePark
    {
        public string GetThemeParkName();

        public Task<Dictionary<string, int>> GetWaitTimeList();
    }
}
