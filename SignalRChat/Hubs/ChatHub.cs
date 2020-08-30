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
            db.Messages.Add(new Messages { Name = user, Message = message, SendDate=DateTime.Now });//Добавляем в таблицу "Messages" нашей базы данных новую запись
            db.SaveChanges();//Сохраняем изменения

            var Messages =  await db.Messages.ToListAsync();
                
            // отправляет параметры
            await Clients.All.SendAsync("ReceiveOne", Messages.Last());/*Первый параметр метода SendAsync() указывает на метод, который будет получать ответ от сервера, 
                                                                           а вторые 2 параметра предаставляют набор значений, которые посылаются в ответе клиенту. То есть метод Send 
                                                                           на клиенте получит значение параметров user и message. То есть наш хаб будет просто получать сообщение и 
                                                                           транслировать его всем подключенным клиентам.*/
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
