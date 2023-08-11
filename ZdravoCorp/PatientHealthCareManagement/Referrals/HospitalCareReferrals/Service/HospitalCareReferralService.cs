using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Repository;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Service
{
    public class HospitalCareReferralService
    {
        private IHospitalCareReferralRepository _hospitalCareReferralRepository;

        public HospitalCareReferralService(IHospitalCareReferralRepository hospitalCareReferralRepository)
        {
            _hospitalCareReferralRepository = hospitalCareReferralRepository;
        }

        public List<HospitalCareReferral> GetAll()
        {
            return _hospitalCareReferralRepository.GetAll();
        }

        public HospitalCareReferral? GetById(string id)
        {
            return _hospitalCareReferralRepository.GetById(id);
        }

        public void Add(HospitalCareReferral referral)
        {
            _hospitalCareReferralRepository.Add(referral);
        }

        public void Delete(HospitalCareReferral referral)
        {
            _hospitalCareReferralRepository.Delete(referral);
        }

        public void Update(HospitalCareReferral referral)
        {
            _hospitalCareReferralRepository.Update(referral);
        }

        public string NextId()
        {
            return _hospitalCareReferralRepository.NextId();
        }

 
    }
}
