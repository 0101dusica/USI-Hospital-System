using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Chats.Model;
using ZdravoCorp.CommunicatonManagement.Polls.Model;

namespace ZdravoCorp.CommunicatonManagement.Chats.Repository
{
    public interface IChatRepository
    {
        List<Chat> GetAll();
        Chat? GetById(string id);
        void Add(Chat poll);
        void Delete(Chat poll);
        void Update(Chat updatedPoll);
        string NextId();
        ObservableCollection<Chat> GetChatMessages(string usernameFrom, string usernameTo);
    }
}
