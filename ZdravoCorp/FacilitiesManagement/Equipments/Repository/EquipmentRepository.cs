using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.FacilitiesManagement.Equipments.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.FacilitiesManagement.Equipments.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private static List<Equipment> _equipments = new List<Equipment>();
        private const string _storagePath = "../../../../Data/Equipments.json";

        private ISerializer<Equipment> _serializer;

        public EquipmentRepository(ISerializer<Equipment> serializer)
        {
            _serializer = serializer;
            _equipments = Load();
        }

        public List<Equipment> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _equipments);
        }

        public List<Equipment> GetAll()
        {
            return _equipments;
        }

        public Equipment? GetById(string id)
        {
            return _equipments.FirstOrDefault(a => a.Id.Equals(id));
        }

        public void Add(Equipment equipment)
        {
            _equipments.Add(equipment);
            Save();
        }

        public void Delete(Equipment equipment)
        {
            _equipments.Remove(equipment);
            Save();
        }

        public void Update(Equipment updatedEquipment)
        {
            var existingEquipment = GetById(updatedEquipment.Id);
            if (existingEquipment != null)
            {
                existingEquipment.Name = updatedEquipment.Name;
                existingEquipment.EquipmentType = updatedEquipment.EquipmentType;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _equipments.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "equipment1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("equipment", ""));
                return $"equipment{lastIdNumber + 1}";
            }
        }



    }
}
