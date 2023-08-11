using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Warehouses.Service
{
    public class WarehouseService
    {
        private IWarehouseRepository _warehouseRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public List<Warehouse> GetAll()
        {
            return _warehouseRepository.GetAll();
        }

        public List<string> GetAllIds()
        {
            return _warehouseRepository.GetAllIds();
        }

        public Warehouse? GetById(string id)
        {
            return _warehouseRepository.GetById(id);
        }

        public void Add(Warehouse warehouse)
        {
            _warehouseRepository.Add(warehouse);
        }

        public void Delete(Warehouse warehouse)
        {
            _warehouseRepository.Delete(warehouse);
        }

        public void Update(Warehouse updatedWarehouse)
        {
            _warehouseRepository.Update(updatedWarehouse);
        }

        public void NextId()
        {
            _warehouseRepository.NextId();
        }

        public List<MedicineItem> GetAllMedicineItems()
        {
            return _warehouseRepository.GetAllMedicineItems();
        }

        public int GetMedicineItemQuantity(string id)
        {
            return _warehouseRepository.GetMedicineItemQuantity(id);
        }

        public bool isMedicineOnCondition(string id)
        {
            return GetAllMedicineItems().Any(medicineItem => medicineItem.MedicineId == id && medicineItem.Quantity > 0);
        }

        public void ReduceMedicineQuantity(string id)
        {
            _warehouseRepository.ReduceMedicineQuantity(id);

        }

        public List<MedicineItem> GetMedicinesInDeficit()
        {
            return _warehouseRepository.GetMedicinesInDeficit();
        }

        public List<MedicineItem> AddInMedicineOrder(MedicineItem newMedicineItem)
        {
            return _warehouseRepository.AddInMedicineOrder(newMedicineItem);
        }

        public MedicineItem? GetMedicineItemById(string id)
        {
            return _warehouseRepository.GetMedicineItemById(id);
        }

        public void AddInMedicineItems(MedicineItem medicineItem)
        {
            _warehouseRepository.AddInMedicineItems(medicineItem);
        }

        public List<MedicineItem> GetItemsOutOfStock()
        {
            return _warehouseRepository.GetItemsOutOfStock();
        }

        public void UpdateMedicineItemConditionStatus(string id)
        {
            _warehouseRepository.UpdateMedicineItemConditionStatus(id);
        }
    }
}
