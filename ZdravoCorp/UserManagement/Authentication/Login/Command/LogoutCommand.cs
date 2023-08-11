using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.UserManagement.Authentication.Login.View;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Login.Commands
{
    public class LogoutCommand : BaseCommand
    {
        public Window MenuView { get; }

        public LogoutCommand(Window menuView)
        {
            MenuView = menuView;
        }

        public LogoutCommand(Window menuView, PatientNotificationService PatientNotificationService)
        {
            MenuView = menuView;
            PatientNotificationService.StopNotificationTimer();
        }
        public override void Execute(object? parameter)
        {
            LoginView loginView = new LoginView();
            MenuView.Close();
            loginView.ShowDialog();
        }
    }
}
