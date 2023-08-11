using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Chats.Model;
using ZdravoCorp.CommunicatonManagement.Polls.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.CommunicatonManagement.Chats.Repository
{
    public class ChatRepository : IChatRepository
    {
        private static List<Chat> _chats = new List<Chat>();
        private const string _storagePath = "../../../Data/Messages.json";

        private ISerializer<Chat> _serializer;


        public ChatRepository(ISerializer<Chat> serializer)
        {
            _serializer = serializer;
            _chats = Load();
        }

        public List<Chat> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _chats);
        }

        public List<Chat> GetAll()
        {
            return _chats;
        }

        public Chat? GetById(string id)
        {
            return _chats.FirstOrDefault(p => p.Id.Equals(id));
        }


        public void Add(Chat chat)
        {
            _chats.Add(chat);
            Save();
        }

        public void Delete(Chat chat)
        {
            _chats.Remove(chat);
            Save();
        }

        public void Update(Chat updatedChat)
        {
            var existingPoll = GetById(updatedChat.Id);
            if (existingPoll != null)
            {
                existingPoll.UsernameFrom = updatedChat.UsernameFrom;
                existingPoll.UsernameTo = updatedChat.UsernameTo;
                existingPoll.TimeSent = updatedChat.TimeSent;
                existingPoll.Message = updatedChat.Message;
                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _chats.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "message1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("message", ""));
                return $"message{lastIdNumber + 1}";
            }
        }

        public ObservableCollection<Chat> GetChatMessages(string usernameFrom, string usernameTo)
        {
            return new ObservableCollection<Chat>(
                _chats.Where(chat =>
                    (chat.UsernameFrom == usernameFrom && chat.UsernameTo == usernameTo) ||
                    (chat.UsernameFrom == usernameTo && chat.UsernameTo == usernameFrom)
                )
            );
        }
    }
}
