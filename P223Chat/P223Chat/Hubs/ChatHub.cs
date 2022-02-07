using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using P223Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P223Chat.Hubs
{
    public class ChatHub:Hub
    {
        private readonly UserManager<AppUser> _userManager;

        public ChatHub(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task SendMessage(string name,string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", name, message,DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                user.ConnectionId = Context.ConnectionId;

                var result = _userManager.UpdateAsync(user).Result;

                Clients.All.SendAsync("UserConnected", user.Id);
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                AppUser user = _userManager.FindByNameAsync(Context.User.Identity.Name).Result;

                user.ConnectionId = null;

                var result = _userManager.UpdateAsync(user).Result;

                Clients.All.SendAsync("UserDisConnected", user.Id);
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
