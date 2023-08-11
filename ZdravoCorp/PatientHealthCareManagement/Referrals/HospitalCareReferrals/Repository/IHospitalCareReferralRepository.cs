using System;
using System.Collections.Generic;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Repository
{
    public interface IHospitalCareReferralRepository
    {
        List<HospitalCareReferral> GetAll();
        HospitalCareReferral? GetById(string id);
        void Add(HospitalCareReferral hospitalCareReferral);
        void Delete(HospitalCareReferral hospitalCareReferral);
        void Update(HospitalCareReferral hospitalCareReferral);
        string NextId();
    }
}
