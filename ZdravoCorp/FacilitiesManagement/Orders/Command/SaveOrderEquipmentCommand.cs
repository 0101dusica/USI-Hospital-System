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
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;

namespace ZdravoCorp.FacilitiesManagement.Orders.Command

{
    public class SaveOrderEquipmentCommand : BaseCommand
    {
        private CreateOrderViewModel _createOrderViewModel { get; set; }
        private OrderService _orderService { get; set; }
        public SaveOrderEquipmentCommand(CreateOrderViewModel createOrderViewModel)
        {
            _createOrderViewModel = createOrderViewModel;
            _orderService = new OrderService(new OrderRepository(new Serializer<Order>()), new WarehouseRepository(new Serializer<Warehouse>()));
        }

        private bool CanExecute(object parameter)
        {
            if (_createOrderViewModel.QuantityInput <= 0)
            {
                MessageBox.Show("You can't order less than 1 equipment!");
                return false;
            }
            return true;

        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                OrderItem orderItem = new OrderItem(_createOrderViewModel.SelectedRow.Item1, (int)_createOrderViewModel.QuantityInput);

                _createOrderViewModel.Order.OrderItems.Add(orderItem);

                MessageBox.Show($"You add {orderItem.Equipment.Name} to Order!");
            }
        }
    }
}
