using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model
{
    public class MedicineItem
    {

        public enum ConditionStatus
        {
            OnStock,
            OutOfStock,
            InProcess
        }

        public string MedicineId { get; set; }
        public int Quantity { get; set; }
        public ConditionStatus Status { get; set; }
        public MedicineItem() { }

        public MedicineItem(string id, int quantity, ConditionStatus status)
        {
            MedicineId = id;
            Quantity = quantity;
            Status = status;
        }
    }
}
