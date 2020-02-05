using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SignalRChat.Models
{
    public class Messages
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
    }
}
