using System;
using System.Collections.Generic;

using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.FacilitiesManagement.Orders.Model;
using ZdravoCorp.FacilitiesManagement.Orders.Repository;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Orders.Service
{
    public class OrderService
    {
        private IOrderRepository _orderRepository;
        private IWarehouseRepository _warehouseRepository;
        public OrderService(IOrderRepository orderRepository, IWarehouseRepository warehouseRepository)
        {
            _orderRepository = orderRepository;
            _warehouseRepository = warehouseRepository;
        }

        public List<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order? GetById(string id)
        {
            return _orderRepository.GetById(id);
        }

        public void Add(Order order)
        {
            _orderRepository.Add(order);
        }

        public void Delete(Order order)
        {
            _orderRepository.Delete(order);
        }

        public void Update(Order updatedOrder)
        {
            _orderRepository.Update(updatedOrder);
        }

        public string NextId()
        {
            return _orderRepository.NextId();
        }

        public void SetTimers(Order order)
        {
            TimeSpan interval = order.TimeSlot.EndTime > DateTime.Now ? order.TimeSlot.EndTime - DateTime.Now : TimeSpan.FromSeconds(1);

            System.Timers.Timer startRenovationTimer = new System.Timers.Timer(interval.TotalMilliseconds);
            startRenovationTimer.Elapsed += (sender, e) => { UpdateInventoryItems(order); };

            startRenovationTimer.AutoReset = false;
            startRenovationTimer.Enabled = true;
        }

        private void UpdateInventoryItems(Order order)
        {
            foreach (OrderItem itemOrder in order.OrderItems)
            {
                bool findItem = false;

                foreach (Warehouse warehouse in _warehouseRepository.GetAll())
                {
                    foreach (InventoryItem item in warehouse.InventoryItems)
                    {
                        if (itemOrder.Equipment.Id.Equals(item.EquipmentId))
                        {
                            item.Quantity = itemOrder.Quantity + item.Quantity;
                            findItem = true;
                        }
                    }

                    if (findItem == false)
                    {
                        InventoryItem inventoryItem = new InventoryItem(itemOrder.Equipment.Id, itemOrder.Quantity, 0);
                        warehouse.InventoryItems.Add(inventoryItem);
                    }

                    _warehouseRepository.Update(warehouse);
                }
            }

            foreach (Order orderStorage in _orderRepository.GetAll())
            {
                if (orderStorage.Id == order.Id)
                {
                    orderStorage.OrderStatus = OrderStatus.Done;
                }
            }

            _orderRepository.Update(order);

        }
    }
}
