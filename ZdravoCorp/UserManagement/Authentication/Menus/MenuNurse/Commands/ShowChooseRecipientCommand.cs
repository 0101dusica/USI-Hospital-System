using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Chats.View;
using ZdravoCorp.CommunicatonManagement.Chats.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands
{
    public class ShowChooseRecipientCommand : BaseCommand
    {
        private User _loggedUser;
        public ShowChooseRecipientCommand(User loggedUser)
        {
            _loggedUser = loggedUser;
        }
        public override void Execute(object? parameter)
        {
            ChooseRecipientView chooseRecipientView = new ChooseRecipientView();
            chooseRecipientView.DataContext = new ChooseRecipientViewModel(_loggedUser);
            chooseRecipientView.Show();
        }
    }
}
