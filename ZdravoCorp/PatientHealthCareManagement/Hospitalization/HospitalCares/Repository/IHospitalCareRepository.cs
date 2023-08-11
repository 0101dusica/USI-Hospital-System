using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;

namespace ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository
{
    public interface IHospitalCareRepository
    {
        List<HospitalCare> GetAll();
        HospitalCare? GetById(string id);
        void Add(HospitalCare hospitalCare);
        void Delete(HospitalCare hospitalCare);
        void Update(HospitalCare hospitalCare);
        string NextId();

        List<HospitalCare> GetHospitalCaresForPatient(string patientUsername);
    }
}
