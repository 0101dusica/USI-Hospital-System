using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Service
{
    public class SpecialistReferralService
    {
        private ISpecialistReferralRepository _specialistReferralRepository;

        private DoctorService _doctorService;
        public SpecialistReferralService(ISpecialistReferralRepository specialistReferralRepository)
        {
            _specialistReferralRepository = specialistReferralRepository;
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
        }

        public List<SpecialistReferral> GetAll()
        {
            return _specialistReferralRepository.GetAll();
        }

        public SpecialistReferral? GetByUsername(string username)
        {
            return _specialistReferralRepository.GetByUsername(username);
        }

        public void Add(SpecialistReferral specialistReferral)
        {
            _specialistReferralRepository.Add(specialistReferral);
        }

        public void Delete(SpecialistReferral specialistReferral)
        {
            _specialistReferralRepository.Delete(specialistReferral);
        }

        public void Update(SpecialistReferral updatedSpecialistRefferal)
        {
            _specialistReferralRepository.Update(updatedSpecialistRefferal);
        }

        public bool IsDoctorWithSpecializationExist(Doctor doctor, Specialization specialization)
        {
            return _doctorService.IsDoctorWithSpecializationExist(doctor, specialization);
        }
        public List<SpecialistReferral> GetPatientsSpecialistRefferals(string username)
        {
            return _specialistReferralRepository.GetPatientsSpecialistReferrals(username);
        }

        public string NextId()
        {
            return _specialistReferralRepository.NextId();
        }
    }
}
