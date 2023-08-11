using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.CommunicatonManagement.Chats.Model
{
    public class Chat
    {

        public string Id { get; set; }
        public string UsernameFrom { get; set; }
        public string UsernameTo { get; set;}
        public string Message { get; set; }
        public DateTime TimeSent { get; set; }

        public Chat(string id, string usernameFrom, string usernameTo, string message, DateTime dateTime)
        {
            this.Id = id;
            this.UsernameFrom = usernameFrom;
            this.UsernameTo = usernameTo;
            this.Message = message;
            this.TimeSent = dateTime;
        }
    }

}
