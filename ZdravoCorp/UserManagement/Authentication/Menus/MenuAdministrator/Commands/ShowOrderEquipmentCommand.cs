using System;
using ZdravoCorp.FacilitiesManagement.Orders.View;
using ZdravoCorp.FacilitiesManagement.Orders.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.Commands
{
    public class ShowOrderEquipmentCommand : BaseCommand
    {
        public ShowOrderEquipmentCommand() { }
        public override void Execute(object? parameter)
        {
            CreateOrderView createOrderView = new CreateOrderView();
            createOrderView.DataContext = new CreateOrderViewModel(createOrderView);
            createOrderView.ShowDialog();
        }
    }
}
