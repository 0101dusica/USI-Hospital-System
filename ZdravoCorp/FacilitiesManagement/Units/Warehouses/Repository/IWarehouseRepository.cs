using System.Collections.Generic;
using ZdravoCorp.FacilitiesManagement.Units.Warehouses.Model;

namespace ZdravoCorp.FacilitiesManagement.Units.Warehouses.Repository
{
    public interface IWarehouseRepository
    {
        List<Warehouse> GetAll();
        List<string> GetAllIds();
        Warehouse? GetById(string id);
        void Add(Warehouse warehouse);
        void Delete(Warehouse warehouse);
        void Update(Warehouse updatedWarehouse);
        string NextId();
        List<MedicineItem> GetAllMedicineItems();
        int GetMedicineItemQuantity(string id);
        void ReduceMedicineQuantity(string id);
        List<MedicineItem> GetMedicinesInDeficit();
        List<MedicineItem> AddInMedicineOrder(MedicineItem newMedicineItem);
        MedicineItem? GetMedicineItemById(string id);
        void AddInMedicineItems(MedicineItem medicineItem);
        List<MedicineItem> GetItemsOutOfStock();
        void UpdateMedicineItemConditionStatus(string id);
    }
}
