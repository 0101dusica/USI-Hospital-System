using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository
{
    public class MedicineRepository : IMedicineRepository
    {
        private static List<Medicine> _medicines = new List<Medicine>();
        private const string _storagePath = "../../../Data/Medicines.json";

        private ISerializer<Medicine> _serializer;


        public MedicineRepository(ISerializer<Medicine> serializer)
        {
            _serializer = serializer;
            _medicines = Load();
        }

        public List<Medicine> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _medicines);
        }

        public List<Medicine> GetAll()
        {
            return _medicines;
        }

        public Medicine? GetById(string id)
        {
            return _medicines.FirstOrDefault(m => m.Id.Equals(id));
        }

        public void Add(Medicine medicine)
        {
            _medicines.Add(medicine);
            Save();
        }

        public void Delete(Medicine medicine)
        {
            _medicines.Remove(medicine);
            Save();
        }

        public void Update(Medicine updatedMedicine)
        {
            var existingMedicine = GetById(updatedMedicine.Id);
            if (existingMedicine != null)
            {
                existingMedicine.Name = updatedMedicine.Name;
                existingMedicine.Ingredients = updatedMedicine.Ingredients;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _medicines.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "medicine1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("medicine", ""));
                return $"medicine{lastIdNumber + 1}";
            }
        }

        public bool IsPatientAllergic(Medicine medicine, List<string> allergies)
        {
            return medicine.Ingredients.Intersect(allergies).Any();
        }

    }
}
