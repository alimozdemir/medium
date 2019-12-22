
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ChatSample.Hubs
{
    public interface IClientList
    {
        void CreateUser(string connectionId);
        void RemoveUser(string connectionId);
        void LatestPing(string connectionId);

        IEnumerable<Client> GetClients();
    }

    public class ClientList : IClientList
    {
        private ConcurrentDictionary<string, Client> _users;
        public ClientList()
        {
            _users = new ConcurrentDictionary<string, Client>();
        }

        public void CreateUser(string connectionId)
        {
            if (!_users.TryAdd(connectionId, new Client(connectionId)))
                throw new Exception("Couldn't add new user to the list.");
        }
        public void RemoveUser(string connectionId)
        {
            // I didn't remove the client from the collection for see the exit date
            if (_users.TryGetValue(connectionId, out var client))
                client.ExitTime = DateTime.Now;
        }
        public void LatestPing(string connectionId)
        {
            if (_users.TryGetValue(connectionId, out var client))
                client.LatestPingTime = DateTime.Now;
        }

        public IEnumerable<Client> GetClients() => _users.Values;
    }
}