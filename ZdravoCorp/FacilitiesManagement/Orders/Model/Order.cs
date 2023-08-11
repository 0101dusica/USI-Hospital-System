using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Timers;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.FacilitiesManagement.Orders.Model
{
    public enum OrderStatus
    {
        InProcess,
        Done
    }
    public class Order
    {
        public string Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Order() { }

        public Order(string id, OrderStatus orderStatus, TimeSlot timeSlot, List<OrderItem> orderItems)
        {
            Id = id;
            OrderStatus = orderStatus;
            TimeSlot = timeSlot;
            OrderItems = orderItems;

        }

        public Order(string id)
        {
            Id = id;
            OrderStatus = OrderStatus.InProcess;
            TimeSlot = new TimeSlot();
            TimeSlot.StartTime = DateTime.Now;
            TimeSlot.EndTime = DateTime.Now.AddHours(24);
            OrderItems = new List<OrderItem>();
        }

        public Order(string id, OrderStatus orderStatus, DateTime date, List<OrderItem> orderItems)
        {
            Id = id;
            OrderStatus = orderStatus;
            TimeSlot = new TimeSlot();
            TimeSlot.StartTime = date;
            TimeSlot.EndTime = date.AddHours(24);
            OrderItems = orderItems;

        }

    }
}
