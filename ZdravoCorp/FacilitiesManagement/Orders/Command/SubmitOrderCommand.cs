using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using ZdravoCorp.Utils.Command;
using ZdravoCorp.FacilitiesManagement.Orders.ViewModel;
using ZdravoCorp.FacilitiesManagement.Orders.Service;
using ZdravoCorp.FacilitiesManagement.Orders.Model;
using ZdravoCorp.FacilitiesManagement.Orders.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Orders.Command

{
    public class SubmitOrderCommand : BaseCommand
    {
        private CreateOrderViewModel _createOrderViewModel { get; set; }
        private OrderService _orderService { get; set; }
        public SubmitOrderCommand(CreateOrderViewModel createOrderViewModel)
        {
            _createOrderViewModel = createOrderViewModel;
            _orderService = new OrderService(new OrderRepository(new Serializer<Order>()), new WarehouseRepository(new Serializer<Warehouse>()));
        }

        public override void Execute(object? parameter)
        {
            _orderService.Add(_createOrderViewModel.Order);

            _orderService.SetTimers(_createOrderViewModel.Order);
            MessageBox.Show("You just successfully made an order!");
        }
    }
}
