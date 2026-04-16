using Common.Constants;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace QueueTimesService
{
    public class QueueTimesServiceManager : IQueueTimesServiceManager
    {
        private static readonly HttpClient client = new HttpClient();
        private List<ParkGroup> parkIdList;

        private const string GET_PARK_ID_URL = "https://queue-times.com/parks.json";
        private const string GET_WAIT_TIME_URL_FRONT = "https://queue-times.com/parks/";
        private const string GET_WAIT_TIME_URL_REAR = "/queue_times.json";

        public QueueTimesServiceManager()
        {
            parkIdList = new List<ParkGroup>();
        }

        public async Task<int> LoadAllParkIds()
        {
            if (parkIdList.Count > 0)
            {
                // 既に取得済みだったら何もせずに返す
                return 0;
            }

            var json = await client.GetStringAsync(GET_PARK_ID_URL);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<List<ParkGroup>>(json, options);
            if (data != null)
            {
                parkIdList.AddRange(data);
            }

            return 0;
        }

        public async Task<int> GetParkId(string parkName)
        {
            if (parkIdList.Count == 0)
            {
                int result = await LoadAllParkIds();
                if (result < 0)
                {
                    return result;
                }
            }

            // parkNameの解析
            // 例  "Universal Parks & Resorts:Universal Studios Japan"
            // 例  "Universal Parks & Resorts:Tokyo Disneyland"
            // 例  "Universal Parks & Resorts:Tokyo DisneySea
            string[] splitParkName = parkName.Split(ServiceConstants.THEME_PARK_NAME_SPLIT);
            if (splitParkName.Length < 2)
            {
                return -1;
            }

            ParkGroup[] parkGroupList = parkIdList.Where(x => x.name == splitParkName[0]).ToArray();
            if (parkGroupList.Length != 1)
            {
                return -1;
            }

            Park[] parkList = parkGroupList[0].parks!.Where(x => x.name == splitParkName[1]).ToArray();
            if (parkList.Length != 1)
            {
                return -1;
            }

            return parkList[0].id;
        }

        public async Task<Dictionary<string, int>> GetWaitTimeList(int themeParkId)
        {
            string url = GET_WAIT_TIME_URL_FRONT + themeParkId.ToString() + GET_WAIT_TIME_URL_REAR;
            var result = new Dictionary<string, int>();

            var json = await client.GetStringAsync(url);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<QueueTimesResponse>(json, options);
            if (data != null)
            {
                foreach (var ride in data.rides!)
                {
                    if (ride != null)
                    {
                        result.Add(ride.name!, (int)ride.wait_time!);
                    }
                }
            }


            return result;
        }
    }
}
