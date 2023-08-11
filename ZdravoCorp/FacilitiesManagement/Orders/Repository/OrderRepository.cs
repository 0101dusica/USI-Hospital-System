using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.FacilitiesManagement.Orders.Model;

namespace ZdravoCorp.FacilitiesManagement.Orders.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private static List<Order> _orders = new List<Order>();
        private const string _storagePath = "../../../Data/Orders.json";

        private ISerializer<Order> _serializer;


        public OrderRepository(ISerializer<Order> serializer)
        {
            _serializer = serializer;
            _orders = Load();
        }

        public List<Order> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _orders);
        }

        public List<Order> GetAll()
        {
            return _orders;
        }

        public Order? GetById(string id)
        {
            return _orders.FirstOrDefault(o => o.Id.Equals(id));
        }

        public void Add(Order order)
        {
            _orders.Add(order);
            Save();
        }

        public void Delete(Order order)
        {
            _orders.Remove(order);
            Save();
        }

        public void Update(Order updatedOrder)
        {
            var existingOrder = GetById(updatedOrder.Id);
            if (existingOrder != null)
            {
                existingOrder.OrderStatus = updatedOrder.OrderStatus;
                existingOrder.TimeSlot = updatedOrder.TimeSlot;
                existingOrder.OrderItems = updatedOrder.OrderItems;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _orders.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "order1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("order", ""));
                return $"order{lastIdNumber + 1}";
            }
        }
    }
}
