using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Chats.Model;
using ZdravoCorp.CommunicatonManagement.Chats.Service;
using ZdravoCorp.CommunicatonManagement.Chats.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.CommunicatonManagement.Chats.Command
{
    public class SendMessageCommand : BaseCommand
    {

        private ChatViewModel _chatViewModel;
        private ChatService _chatService;
        public SendMessageCommand(ChatViewModel chatViewModel, ChatService chatService)
        {
            _chatViewModel = chatViewModel;
            _chatService = chatService;
        }

        public bool CanExecute(object? parameter)
        {
            return _chatViewModel.Message!.Length > 0;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Chat chat = new Chat(_chatService.NextId(), _chatViewModel.UsernameFrom, _chatViewModel.UsernameTo,
                    _chatViewModel.Message!,
                    DateTime.Now);
                _chatService.Add(chat);
                MessageBox.Show("Message sent successfully");
                _chatViewModel.LoadListBox();
            }
            else
            {
                MessageBox.Show("You can not send empty message!");
            }
        }
    }
}
