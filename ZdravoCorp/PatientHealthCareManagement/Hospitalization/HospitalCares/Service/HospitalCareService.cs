using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Repository;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Service;
using ZdravoCorp.PatientHealthCareManagement.Visits.Model;
using ZdravoCorp.PatientHealthCareManagement.Visits.Repository;
using ZdravoCorp.PatientHealthCareManagement.Visits.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.HospitalCares.Service
{
    public class HospitalCareService
    {
        private IHospitalCareRepository _hospitalCareRepository;
        private HospitalCareReferralService _hospitalCareReferralService;
        private VisitService _visitService;
        

        public HospitalCareService(IHospitalCareRepository hospitalCareRepository)
        {
            _hospitalCareRepository = hospitalCareRepository;
            _hospitalCareReferralService = new HospitalCareReferralService(new HospitalCareReferralRepository(new Serializer<HospitalCareReferral>()));
            _visitService = new VisitService(new VisitRepository(new Serializer<Visit>()));

        }

        public List<HospitalCare> GetAll()
        {
            return _hospitalCareRepository.GetAll();
        }

        public HospitalCare? GetById(string id)
        {
            return _hospitalCareRepository.GetById(id);
        }

        public void Add(HospitalCare hospitalCare)
        {
            _hospitalCareRepository.Add(hospitalCare);
        }

        public void Delete(HospitalCare hospitalCare)
        {
            _hospitalCareRepository.Delete(hospitalCare);
        }

        public void Update(HospitalCare updatedHospitalCare)
        {
            _hospitalCareRepository.Update(updatedHospitalCare);
        }
        public string NextId()
        {
            return _hospitalCareRepository.NextId();
        }

        public List<HospitalCare> GetHospitalCaresForPatient(string patientUsername)
        {
            return _hospitalCareRepository.GetHospitalCaresForPatient(patientUsername);
        }

        public HospitalCareReferral GetHospitalCareReferral(string id)
        {
            return _hospitalCareReferralService.GetById(id);

        }

        public List<Visit> GetVisitsForHospitalCare(HospitalCare hospitalCare)
        {
           
            return hospitalCare.VisitIds.Select(visitId => _visitService.GetById(visitId)).Where(visit => visit != null).ToList();
        }

    }
}
