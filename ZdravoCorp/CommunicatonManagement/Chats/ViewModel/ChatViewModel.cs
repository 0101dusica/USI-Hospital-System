using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.CommunicatonManagement.Chats.Command;
using ZdravoCorp.CommunicatonManagement.Chats.Model;
using ZdravoCorp.CommunicatonManagement.Chats.Repository;
using ZdravoCorp.CommunicatonManagement.Chats.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.CommunicatonManagement.Chats.ViewModel
{
    public class ChatViewModel : BaseViewModel
    {
        private ChatService _chatService;

        public ICommand SendMessageCommand { get; }

        public string UsernameTo { get; set; }
        public string UsernameFrom { get; set; }

        private ObservableCollection<Chat> _chats = new ObservableCollection<Chat>();
        public ObservableCollection<Chat> Chats
        {
            get => _chats;
            set
            {
                _chats = value;
                OnPropertyChanged(nameof(Chats));
            }
        }

        private string? _message;
        public string? Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        public ChatViewModel(string usernameFrom, string usernameTo)
        {
            _chatService = new ChatService(new ChatRepository(new Serializer<Chat>()));
            //that who is current logged
            UsernameFrom = usernameFrom;
            //that who was chosen in past view
            UsernameTo = usernameTo;
            LoadListBox();

            SendMessageCommand = new SendMessageCommand(this, _chatService);
        }

        public void LoadListBox()
        {
            Chats = new ObservableCollection<Chat>(_chatService.GetChatMessages(UsernameFrom, UsernameTo));
        }

       
    }
}
