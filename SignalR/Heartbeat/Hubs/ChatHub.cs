using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace ChatSample.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IClientList _list;

        public ChatHub(IClientList list)
        {
            _list = list;
        }
        public override Task OnConnectedAsync()
        {
            _list.CreateUser(Context.ConnectionId);
            Heartbeat();
            return Task.CompletedTask;
        }

        public override Task OnDisconnectedAsync(Exception ex)
        {
            _list.RemoveUser(Context.ConnectionId);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Initiate the heartbeat callback.
        /// </summary>
        private void Heartbeat()
        {
            var heartbeat = Context.Features.Get<IConnectionHeartbeatFeature>();
            
            heartbeat.OnHeartbeat(state => {
                (HttpContext context ,string connectionId) = ((HttpContext, string))state;
                var ClientList = context.RequestServices.GetService<IClientList>();
                ClientList.LatestPing(connectionId);
            }, (Context.GetHttpContext(), Context.ConnectionId));
        }

        public async Task Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }
    }
}