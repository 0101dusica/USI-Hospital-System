using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Transfers.View;
using ZdravoCorp.FacilitiesManagement.Transfers.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.Commands
{
    public class ShowEquipmentDistributionCommand : BaseCommand
    {
        public ShowEquipmentDistributionCommand() { }

        public override void Execute(object? parameter)
        {
            EquipmentDistributionView equipmentDistributionView = new EquipmentDistributionView();
            equipmentDistributionView.DataContext = new EquipmentDistributionViewModel(equipmentDistributionView);
            equipmentDistributionView.ShowDialog();
        }
    }
}
