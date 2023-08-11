using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;
using ZdravoCorp.Utils.Serializer;
using static ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model.MedicineItem;

namespace ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private static List<Warehouse> _warehouses = new List<Warehouse>();
        private const string _storagePath = "../../../Data/Warehouses.json";

        private ISerializer<Warehouse> _serializer;


        public WarehouseRepository(ISerializer<Warehouse> serializer)
        {
            _serializer = serializer;
            _warehouses = Load();
        }

        public List<Warehouse> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _warehouses);
        }

        public List<Warehouse> GetAll()
        {
            return _warehouses;
        }

        public List<string> GetAllIds()
        {
            return _warehouses.Select(warehouse => warehouse.Id).ToList();
        }

        public Warehouse? GetById(string id)
        {
            return _warehouses.FirstOrDefault(w => w.Id == id);
        }

        public void Add(Warehouse warehouse)
        {
            _warehouses.Add(warehouse);
            Save();
        }

        public void Delete(Warehouse warehouse)
        {
            _warehouses.Remove(warehouse);
            Save();
        }

        public void Update(Warehouse updatedWarehouse)
        {
            var existingWarehouse = GetById(updatedWarehouse.Id);
            if (existingWarehouse != null)
            {
                existingWarehouse.InventoryItems = updatedWarehouse.InventoryItems;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _warehouses.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "warehouse1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("warehouse", ""));
                return $"warehouse{lastIdNumber + 1}";
            }
        }

        public List<MedicineItem> GetAllMedicineItems()
        {
            return _warehouses.SelectMany(warehouse => warehouse.MedicineItems).ToList();
        }

        public int GetMedicineItemQuantity(string id)
        {
            return _warehouses.SelectMany(warehouse => warehouse.MedicineItems)
                      .FirstOrDefault(medicineItem => medicineItem.MedicineId.Equals(id))
                      ?.Quantity ?? 0;
        }

        public void ReduceMedicineQuantity(string id)
        {
            MedicineItem medicineItem = GetAllMedicineItems().FirstOrDefault(item => item.MedicineId.Equals(id));
            if (medicineItem != null)
            {
                medicineItem.Quantity--;
                Save();
            }
        }

        public List<MedicineItem> GetMedicinesInDeficit()
        {
            return GetAllMedicineItems().Where(item => item.Quantity < 5).ToList();
        }

        public List<MedicineItem> AddInMedicineOrder(MedicineItem newMedicineItem)
        {
            List<MedicineItem> medicineItemOrder = new List<MedicineItem>();
            medicineItemOrder.Add(newMedicineItem);
            return medicineItemOrder;
        }

        public MedicineItem? GetMedicineItemById(string id)
        {
            return _warehouses
                .SelectMany(warehouse => warehouse.MedicineItems)
                .FirstOrDefault(medicineItem => medicineItem.MedicineId.Equals(id));
        }

        public void AddInMedicineItems(MedicineItem medicineItem)
        {
            foreach (Warehouse warehouse in _warehouses)
            {
                MedicineItem? medicine = warehouse.MedicineItems.FirstOrDefault(m => m.MedicineId.Equals(medicineItem.MedicineId));
                if (medicine != null)
                {
                    medicine.Quantity = medicineItem.Quantity;
                    medicine.Status = ConditionStatus.OnStock;
                }
            }
            Save();
        }


        public List<MedicineItem> GetItemsOutOfStock()
        {
            return _warehouses
                .SelectMany(warehouse => warehouse.MedicineItems)
                .Where(item => item.Status.Equals(ConditionStatus.OutOfStock))
                .ToList();
        }

        public void UpdateMedicineItemConditionStatus(string id)
        {
            var itemToUpdate = GetMedicineItemById(id);

            if (itemToUpdate != null)
            {
                itemToUpdate.Status = ConditionStatus.OutOfStock;
                Save();
            }
        }

    }
}
