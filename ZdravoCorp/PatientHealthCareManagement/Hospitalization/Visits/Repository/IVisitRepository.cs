using System.Collections.Generic;
using ZdravoCorp.PatientHealthCareManagement.Visits.Model;

namespace ZdravoCorp.PatientHealthCareManagement.Visits.Repository
{
    public interface IVisitRepository
    {
        List<Visit> GetAll();
        Visit? GetById(string id);
        void Add(Visit visit);
        void Delete(Visit visit);
        void Update(Visit visit);
    }
}
