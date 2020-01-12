using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Core
{
    //[HubName("Hostsync")]
    public class ChatHub: Hub
    {
        public Task SendMessage(string user, string message)
        {
            string ConnectionId = Context.ConnectionId;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task Test(string message)
        {
            string ConnectionId = Context.ConnectionId;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return Clients.All.SendAsync("Test",message);
        }


        public Task GetConnectionId()
        {
            string ConnectionId = Context.ConnectionId;
            return Clients.Caller.SendAsync("GetConnectionId", ConnectionId);
        }
    }
}
