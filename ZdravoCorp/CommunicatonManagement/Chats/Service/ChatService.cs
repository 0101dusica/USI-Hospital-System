using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Chats.Model;
using ZdravoCorp.CommunicatonManagement.Chats.Repository;
using ZdravoCorp.CommunicatonManagement.Polls.Model;
using ZdravoCorp.CommunicatonManagement.Polls.Repository;

namespace ZdravoCorp.CommunicatonManagement.Chats.Service
{
    public class ChatService
    {
        private IChatRepository _chatRepository;
        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public List<Chat> GetAll()
        {
            return _chatRepository.GetAll();
        }

        public Chat? GetById(string id)
        {
            return _chatRepository.GetById(id);
        }

        public void Add(Chat chat)
        {
            _chatRepository.Add(chat);
        }

        public void Delete(Chat chat)
        {
            _chatRepository.Delete(chat);
        }

        public void Update(Chat updatedChat)
        {
            _chatRepository.Update(updatedChat);
        }

        public string NextId()
        {
            return _chatRepository.NextId();
        }

        public ObservableCollection<Chat> GetChatMessages(string usernameFrom, string usernameTo)
        {
            return _chatRepository.GetChatMessages(usernameFrom, usernameTo);
        }
    }
}
