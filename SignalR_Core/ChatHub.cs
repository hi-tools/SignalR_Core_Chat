using Microsoft.AspNetCore.SignalR;
using SignalR_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_Core
{
    //[HubName("Hostsync")]
    public class ChatHub_Back : Hub
    {
        public Task SendMessage(string user, string message)
        {
            string ConnectionId = Context.ConnectionId;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageOne(string user, string message, string connectionId)
        {
            string ConnectionId = Context.ConnectionId;
            string userId = Context.UserIdentifier;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            //return Clients.Client(connectionId).SendAsync("ReceiveMessageOne", user, message);

            return Clients.Client(connectionId).SendAsync("ReceiveMessageOne", user, message);
            //return Clients.Client(connectionId).SendAsync("ReceiveMessageOne", user, message);

        }

        public Task Test(string message)
        {
            string ConnectionId = Context.ConnectionId;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return Clients.All.SendAsync("Test", message);
        }


        public Task GetConnectionId()
        {
            string ConnectionId = Context.ConnectionId;
            return Clients.Caller.SendAsync("GetConnectionId", ConnectionId);
        }
    }
    public class ChatHub : Hub<IChatHub>
    {
        static List<UserView> lst = new List<UserView>();
        //public Task SendMessage(string user, string message)
        //{
        //    string ConnectionId = Context.ConnectionId;
        //    string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
        //    return Clients.All.ReceiveMessage(user, message);
        //}

        public override Task OnConnectedAsync()
        {
            string ConnectionId = Context.ConnectionId;
            string username = Context.GetHttpContext().Request.Query["username"].ToString();
            lst.Add(new UserView() { ConnectionId = ConnectionId, UserName = username });

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            string ConnectionId = Context.ConnectionId;
            var username = Context.GetHttpContext().Request.Query["username"].ToString();
            var t = lst.FirstOrDefault(a => a.UserName == username && a.ConnectionId == ConnectionId);
            if (t != null)
            {
                lst.Remove(t);
            }
            return base.OnDisconnectedAsync(exception);
        }
        public Task SendMessage(string user, string message)
        {
            string ConnectionId = Context.ConnectionId;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return Clients.All.ReceiveMessage(user, message);
        }

        public Task SendMessageOne(string user, string message, string recieverUser)
        {
            string ConnectionId = Context.ConnectionId;
            string userId = Context.UserIdentifier;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            //return Clients.Client(connectionId).SendAsync("ReceiveMessageOne", user, message);

            var t = lst.FirstOrDefault(a => a.UserName == recieverUser);
            if (t != null)
            {
                return Clients.Clients(t.ConnectionId, ConnectionId).ReceiveMessageOne(user, message);
            }
            return Clients.All.ReceiveMessageOne(user, "کاربر یافت نشد");
            //return Clients.Client(connectionId).SendAsync("ReceiveMessageOne", user, message);

        }

        public Task Test(string message)
        {
            string ConnectionId = Context.ConnectionId;
            string userAgent = Context.GetHttpContext().Request.Headers["User-Agent"].ToString();
            return Clients.All.Test(message);
        }


        public Task GetConnectionId()
        {
            string ConnectionId = Context.ConnectionId;
            return Clients.Caller.GetConnectionId(ConnectionId);
        }

        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.ReceiveMessage(user, message);
        //}

        //public Task SendMessageToCaller(string user, string message)
        //{
        //    return Clients.Caller.ReceiveMessage(user, message);
        //}
    }

    public interface IChatHub
    {
        Task ReceiveMessage(string user, string message);
        Task ReceiveMessageOne(string user, string message);
        Task Test(string message);
        Task GetConnectionId(string ConnectionId);


    }
}
