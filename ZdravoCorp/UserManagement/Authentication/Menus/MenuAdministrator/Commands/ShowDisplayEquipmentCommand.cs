using System;
using ZdravoCorp.FacilitiesManagement.Equipments.View;
using ZdravoCorp.FacilitiesManagement.Equipments.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.Commands
{
    public class ShowDisplayEquipmentCommand : BaseCommand
    {
        public ShowDisplayEquipmentCommand() { }

        public override void Execute(object? parameter)
        {
            EquipmentDisplayView equipmentDisplayView = new EquipmentDisplayView();
            equipmentDisplayView.DataContext = new EquipmentDisplayViewModel(equipmentDisplayView);
            equipmentDisplayView.ShowDialog();
        }
    }
}
