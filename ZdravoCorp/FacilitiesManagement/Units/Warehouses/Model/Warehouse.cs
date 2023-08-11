using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model
{
    public class Warehouse
    {
        public string Id { get; set; }
        public List<InventoryItem> InventoryItems { get; set; }
        public List<MedicineItem> MedicineItems { get; set; }

        public Warehouse() { }

        public Warehouse(string id, List<InventoryItem> inventoryItems, List<MedicineItem> medicineItems)
        {
            Id = id;
            InventoryItems = inventoryItems;
            MedicineItems = medicineItems;
        }

    }
}
