using System.Windows.Input;
using ZdravoCorp.UserManagement.Authentication.Login.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.View;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.ViewModel
{
    public class MenuNurseViewModel : BaseViewModel
    {
        //View
        private MenuNurseView _menuNurseView { get; }

        //Comands
        public ICommand LogoutCommand { get; }
        public ICommand PatientsCommand { get; }
        public ICommand AddEmergencyAppotinmentCommand { get; }
        public ICommand ShowNurseAppointmentWindowCommand { get; }
        public ICommand ShowNurseDispensingWindowCommand { get; }
        public ICommand ShowChooseRecipientCommand { get; }

        //Constructor
        public MenuNurseViewModel(MenuNurseView menuNurseView, Nurse loggedNurse)
        {
            _menuNurseView = menuNurseView;
            LogoutCommand = new LogoutCommand(_menuNurseView);
            PatientsCommand = new PatientsCommand();
            AddEmergencyAppotinmentCommand = new AddEmergencyAppotinmentCommand();
            ShowNurseAppointmentWindowCommand = new ShowNurseAppointmentWindowCommand();
            ShowNurseDispensingWindowCommand = new ShowNurseDispensingWindowCommand();
            ShowChooseRecipientCommand = new ShowChooseRecipientCommand(loggedNurse);

        }
    }
}
