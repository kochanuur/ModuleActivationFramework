using System.Text.Json;

namespace QueueTimesService
{
    public class ParkGroup
    {
        public string? name { get; set; }
        public List<Park>? parks { get; set; }
    }

    public class Park
    {
        public int id { get; set; }
        public string? name { get; set; }
    }

    public class QueueTimesResponse
    {
        public List<Ride>? rides { get; set; }
    }


    public class Ride
    {
        public string? name { get; set; }
        public int? wait_time { get; set; }
        public bool is_open { get; set; }
    }
}
