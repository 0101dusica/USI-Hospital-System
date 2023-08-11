using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service
{
    public class MedicineService
    {
        private IMedicineRepository _medicineRepository;
        public MedicineService(IMedicineRepository medicineRepository)
        {
            _medicineRepository = medicineRepository;
        }

        public List<Medicine> GetAll()
        {
            return _medicineRepository.GetAll();
        }

        public Medicine? GetById(string id)
        {
            return _medicineRepository.GetById(id);
        }

        public void Add(Medicine medicine)
        {
            _medicineRepository.Add(medicine);
        }

        public void Delete(Medicine medicine)
        {
            _medicineRepository.Delete(medicine);
        }

        public void Update(Medicine updatedMedicine)
        {
            _medicineRepository.Update(updatedMedicine);
        }

        public void NextId()
        {
            _medicineRepository.NextId();
        }

        public bool IsPatientAllergic(Medicine medicine, List<string> allergies)
        {
            return _medicineRepository.IsPatientAllergic(medicine, allergies);
        }
    }
}
