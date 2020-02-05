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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;


namespace SignalRChat.Hubs
{
    [Authorize]
    public class ChatHub: Hub
    {
        public ConfigurationBuilder builder = new ConfigurationBuilder();
        
        public async Task SendMessage(string user, string message)// принимает параметры
        {
            // Получаем строку подключения к базе из нашего json файла
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("SignalRChatContext");
            // Передаём строку подключения в конструктор контекста
            var optionsBuilder = new DbContextOptionsBuilder<SignalRChatContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;
            // Создаём объект контекста
            using SignalRChatContext db = new SignalRChatContext(options);
            
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
            //var context = this.Context.GetHttpContext();
            await Clients.All.SendAsync("Notify", $"{userName} вошёл в чат");

            // Получаем строку подключения к базе из нашего json файла
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("SignalRChatContext");
            // Передаём строку подключения в конструктор контекста
            var optionsBuilder = new DbContextOptionsBuilder<SignalRChatContext>();
            var options = optionsBuilder.UseSqlServer(connectionString).Options;
            // Создаём объект контекста
            using SignalRChatContext db = new SignalRChatContext(options);

            var Messages = await db.Messages.ToListAsync();// Получаем список записей из нашей таблицы асинхронно
            await Clients.Caller.SendAsync("Receive", Messages);

            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = Context.User;
            var userName = user.Identity.Name;
            //var context = this.Context.GetHttpContext();
            await Clients.All.SendAsync("Notify", $"{userName} покинул чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
