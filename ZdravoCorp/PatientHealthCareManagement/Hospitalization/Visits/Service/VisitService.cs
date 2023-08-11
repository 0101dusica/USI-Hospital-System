using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Visits.Model;
using ZdravoCorp.PatientHealthCareManagement.Visits.Repository;

namespace ZdravoCorp.PatientHealthCareManagement.Visits.Service
{
    public class VisitService
    {
        private IVisitRepository _visitRepository;

        public VisitService(IVisitRepository visitRepository)
        {
            _visitRepository = visitRepository;
        }
        public List<Visit> GetAll()
        {
            return _visitRepository.GetAll();
        }

        public Visit? GetById(string id)
        {
            return _visitRepository.GetById(id);
        }

        public void Add(Visit visit)
        {
            _visitRepository.Add(visit);
        }

        public void Delete(Visit visit)
        {
            _visitRepository.Delete(visit);
        }

        public void Update(Visit visit)
        {
            _visitRepository.Update(visit);
        }
    }
}
