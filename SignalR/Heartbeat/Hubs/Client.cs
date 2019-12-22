using System;

namespace ChatSample.Hubs
{
    public class Client
    {
        public Client(string connectionId)
        {
            ConnectionId = connectionId;
            EntranceTime = DateTime.Now;
        }
        public string ConnectionId { get; }

        public DateTime EntranceTime { get; }
        public DateTime LatestPingTime { get; set; }
        public DateTime ExitTime { get; internal set; }
    }
}