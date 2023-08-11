using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.FacilitiesManagement.Equipments.Service;
using ZdravoCorp.FacilitiesManagement.Orders.Service;
using ZdravoCorp.FacilitiesManagement.Orders.Model;
using ZdravoCorp.FacilitiesManagement.Orders.Command;
using ZdravoCorp.FacilitiesManagement.Orders.View;

using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.FacilitiesManagement.Equipments.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Model;
using ZdravoCorp.FacilitiesManagement.Units.Rooms.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Orders.Repository;

namespace ZdravoCorp.FacilitiesManagement.Orders.ViewModel
{
    public class CreateOrderViewModel : BaseViewModel
    {
        private EquipmentService _equipmentService;

        private OrderService _orderService;
        public string RoomId { get; set; }
        public List<Tuple<Equipment, int>> Equipments { get; set; }
        public int? QuantityInput { get; set; }

        public Order Order { get; set; }
        public ICommand SaveOrder { get; }
        public ICommand SubmitOrder { get; }

        private Tuple<Equipment, int> _selectedRow;
        public Tuple<Equipment, int> SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                OnPropertyChanged(nameof(SelectedRow));
            }
        }

        public CreateOrderView CreateOrderView { get; set; }
        public CreateOrderViewModel(CreateOrderView createOrderView)
        {
            _equipmentService = new EquipmentService(new EquipmentRepository(new Serializer<Equipment>()), new RoomRepository(new Serializer<Room>()), new WarehouseRepository(new Serializer<Warehouse>()));
            _orderService = new OrderService(new OrderRepository(new Serializer<Order>()), new WarehouseRepository(new Serializer<Warehouse>()));

            CreateOrderView = createOrderView;

            Equipments = _equipmentService.GetDeficitDynamicEquipment();
            List<OrderItem> orderItems = new List<OrderItem>();
            Order = new Order(_orderService.NextId(), OrderStatus.InProcess, DateTime.Now, orderItems);

            SaveOrder = new SaveOrderEquipmentCommand(this);
            SubmitOrder = new SubmitOrderCommand(this);

        }

    }
}
