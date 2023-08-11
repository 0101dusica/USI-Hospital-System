using System.Collections.Generic;
using ZdravoCorp.FacilitiesManagement.Orders.Model;

namespace ZdravoCorp.FacilitiesManagement.Orders.Repository
{
    public interface IOrderRepository
    {
        List<Order> GetAll();
        Order? GetById(string id);
        void Add(Order order);
        void Delete(Order order);
        void Update(Order updatedOrder);
        string NextId();
    }
}
