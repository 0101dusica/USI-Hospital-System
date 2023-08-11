using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Orders.Model
{
    public class OrderItem
    {
        public Equipment Equipment { get; set; }
        public int Quantity { get; set; }

        public OrderItem() { }

        public OrderItem(Equipment equipment, int quantity)
        {
            Quantity = quantity;
            Equipment = equipment;
        }


    }
}
