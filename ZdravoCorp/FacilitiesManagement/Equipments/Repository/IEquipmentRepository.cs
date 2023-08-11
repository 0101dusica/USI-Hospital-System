using System.Collections.Generic;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;

namespace ZdravoCorp.FacilitiesManagement.Equipments.Repository
{
    public interface IEquipmentRepository
    {
        List<Equipment> GetAll();
        Equipment? GetById(string id);
        void Add(Equipment equipment);
        void Delete(Equipment equipment);
        void Update(Equipment updatedEquipment);
        string NextId();
    }
}
