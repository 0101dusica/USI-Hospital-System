using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Chats.View;
using ZdravoCorp.CommunicatonManagement.Chats.ViewModel;
using ZdravoCorp.UserManagement;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.CommunicatonManagement.Chats.Command
{
    public class ShowChatCommand : BaseCommand
    {
        private ChooseRecipientViewModel _chooseRecipientViewModel;

        public ShowChatCommand(ChooseRecipientViewModel chooseRecipientViewModel)
        {
            _chooseRecipientViewModel = chooseRecipientViewModel;
        }

        public bool CanExecute(object? parameter)
        {
            if (_chooseRecipientViewModel.CheckIfBothSelected() ||
                _chooseRecipientViewModel.CheckIfBothNotSelected()) return false;
            return true;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                User selectedUser = _chooseRecipientViewModel.CheckWhoIsSelected();
                ChatView chatView = new ChatView();
                chatView.DataContext = new ChatViewModel(_chooseRecipientViewModel.LoggedUser.Username, selectedUser.Username);
                chatView.ShowDialog();
            }
        }
    }
}
