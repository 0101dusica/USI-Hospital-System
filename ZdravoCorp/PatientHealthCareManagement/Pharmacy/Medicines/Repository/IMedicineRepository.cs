using System.Collections.Generic;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository
{
    public interface IMedicineRepository
    {
        List<Medicine> GetAll();
        Medicine? GetById(string id);
        void Add(Medicine medicine);
        void Delete(Medicine medicine);
        void Update(Medicine medicine);
        string NextId();
        bool IsPatientAllergic(Medicine medicine, List<string> allergies);
    }
}
