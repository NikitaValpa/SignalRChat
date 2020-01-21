using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using SignalRChat.Data;

namespace SignalRChat.Components
{
    public class Timer: ViewComponent
    {
        private readonly SignalRChatContext _context; // Экземпляр службы "SignalRChatContext" реализует обращение к таблице "Messages", через класс "SignalRChatContext"

        public Timer(SignalRChatContext context)// Внедряем зависимость в конструктор
        {
            _context = context;
        }

        public IList<Models.Messages> Messages { get; set; }// Создаём автосвойство в классе, которое используется на страничке и в методах бэкенда этой странички, это свойство возвращает список с экземплярами класса модели "Messages"
                                                            // Другими словами, это список объектов "Messages"
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Messages = await _context.Messages.ToListAsync();// Присваеваем нашему свойству контекст таблицы Messages нашей базы данных преобразуя в список
            return View(Messages);//Возвращаем представление в виде списка
        }
    }
}
