using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.UserManagement.Administrators.Service;
using ZdravoCorp.UserManagement.Authentication.Login.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.View;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.ViewModel
{
    public class MenuAdministratorViewModel : BaseViewModel
    {

        private AdministratorService _administratorService;
        private MenuAdministratorView _menuAdministratorView { get; set; }
        public ICommand DisplayEquipment { get; }
        public ICommand OrderEquipment { get; }
        public ICommand EquipmentDistribution { get; }
        public ICommand SchedulingRenovation { get; }
        public ICommand LogoutCommand { get; }

        public MenuAdministratorViewModel(AdministratorService administratorService, MenuAdministratorView menuAdministratorView)
        {
            _administratorService = administratorService;
            _menuAdministratorView = menuAdministratorView;

            DisplayEquipment = new ShowDisplayEquipmentCommand();
            OrderEquipment = new ShowOrderEquipmentCommand();
            EquipmentDistribution = new ShowEquipmentDistributionCommand();
            SchedulingRenovation = new ShowScheduleRenovationViewCommand();

            LogoutCommand = new LogoutCommand(_menuAdministratorView);
        }
    }
}
