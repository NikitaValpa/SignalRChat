using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;


namespace SignalRChat.Hubs
{
    public class ChatHub: Hub
    {
        public async Task SendMessage(string user, string message)// принимает параметры
        {
            // отправляет параметры
            await Clients.All.SendAsync("Receive", user, message);/*Первый параметр метода SendAsync() указывает на метод, который будет получать ответ от сервера, 
                                                                           а вторые 2 параметра предаставляют набор значений, которые посылаются в ответе клиенту. То есть метод Send 
                                                                           на клиенте получит значение параметров user и message. То есть наш хаб будет просто получать сообщение и 
                                                                           транслировать его всем подключенным клиентам.*/
        }
        public override async Task OnConnectedAsync()
        {
            var context = this.Context.GetHttpContext();
            await Clients.All.SendAsync("Notify", $"{context.Connection.RemoteIpAddress.ToString()} вошёл в чат");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var context = this.Context.GetHttpContext();
            await Clients.All.SendAsync("Notify", $"{context.Connection.RemoteIpAddress.ToString()} покинул чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}
