using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignalRChat.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRChat.Models
{
    public class SeeData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new SignalRChatContext(serviceProvider.GetRequiredService<DbContextOptions<SignalRChatContext>>());
            // Смотрим все записи в таблице
            if (context.Messages.Any())
            {
                return;// Если бд не пустая, возвращаем инициализатор
            }
        }
    }
}
