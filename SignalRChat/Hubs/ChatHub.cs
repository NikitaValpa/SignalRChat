using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using SignalRChat.Data;
using SignalRChat.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace SignalRChat.Hubs
{
    public class ChatHub: Hub
    {

        public ConfigurationBuilder builder = new ConfigurationBuilder();
        private readonly ILogger<ChatHub> _logger;
        private readonly SignalRChatContext db;
        public static List<string> UserNames = new List<string>();
        public ChatHub(ILogger<ChatHub> logger, SignalRChatContext context)
        {
            _logger = logger;
            db = context;
        }

        public async Task SendMessage(string user, string message)// принимает параметры
        {
            var Message = new Messages { Name = user, Message = message, SendDate = DateTime.Now };
            
            // отправляет ответ
            await Clients.All.SendAsync("ReceiveOne", Message);

            db.Messages.Add(Message);//Добавляем в таблицу "Messages" нашей базы данных новую запись
            await db.SaveChangesAsync();//Сохраняем изменения

            
                

        }
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;
            var userName = user.Identity.Name;
            var Messages = await db.Messages.ToListAsync();// Получаем список записей из нашей таблицы асинхронно
            //var context = this.Context.GetHttpContext();
            
            var AddUser = UserNames.IndexOf(userName, 0);
            if (AddUser == -1) {
                UserNames.Add(userName);
            }

            if (UserNames.Any()) {
                await Clients.All.SendAsync("Notify", UserNames);
            }

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User;
            var userName = user.Identity.Name;
            //var context = this.Context.GetHttpContext();
            
            var DeleteUser = UserNames.IndexOf(userName, 0);
            if (DeleteUser != -1) {
                UserNames.RemoveAt(DeleteUser);
            }

            if (UserNames.Any()) {
                await Clients.Others.SendAsync("Notify", UserNames);
            }
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}
