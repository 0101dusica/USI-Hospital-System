using System;
using System.Collections.Generic;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Repository
{
    public interface ISpecialistReferralRepository
    {
        List<SpecialistReferral> GetAll();
        SpecialistReferral? GetByUsername(string patientUsername);
        void Add(SpecialistReferral specialistReferral);
        void Delete(SpecialistReferral specialistReferral);
        void Update(SpecialistReferral specialistReferral);
        List<SpecialistReferral> GetPatientsSpecialistReferrals(string username);
        string NextId();
    }
}
