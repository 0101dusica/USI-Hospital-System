using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Repository
{
    public class SpecialistReferralRepository : ISpecialistReferralRepository
    {
        private static List<SpecialistReferral> _specialistRefferals = new List<SpecialistReferral>();
        private const string _storagePath = "../../../Data/SpecialistReferrals.json";

        private ISerializer<SpecialistReferral> _serializer;


        public SpecialistReferralRepository(ISerializer<SpecialistReferral> serializer)
        {
            _serializer = serializer;
            _specialistRefferals = Load();
        }

        public List<SpecialistReferral> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _specialistRefferals);
        }

        public List<SpecialistReferral> GetAll()
        {
            return _specialistRefferals;
        }

        public SpecialistReferral? GetByUsername(string patientUsername)
        {
            return _specialistRefferals.SingleOrDefault(sr => sr.PatientUsername.Equals(patientUsername));
        }


        public void Add(SpecialistReferral specialistReferral)
        {
            _specialistRefferals.Add(specialistReferral);
            Save();
        }

        public void Delete(SpecialistReferral specialistReferral)
        {
            _specialistRefferals.Remove(specialistReferral);
            Save();
        }

        public void Update(SpecialistReferral updatedSpecialistRefferal)
        {
            var existingSpecialistRefferal = GetByUsername(updatedSpecialistRefferal.PatientUsername);
            if (existingSpecialistRefferal != null)
            {
                existingSpecialistRefferal.FromDoctor = updatedSpecialistRefferal.FromDoctor;
                existingSpecialistRefferal.ToDoctor = updatedSpecialistRefferal.ToDoctor;
                existingSpecialistRefferal.SpecialistReferralStatus = updatedSpecialistRefferal.SpecialistReferralStatus;
                Save();
            }
        }
        public List<SpecialistReferral> GetPatientsSpecialistReferrals(string username)
        {
            return GetAll()
                .Where(specialistReferral => specialistReferral.PatientUsername.Equals(username) && specialistReferral.SpecialistReferralStatus.Equals(SpecialistReferralStatus.Created))
            .ToList();
        }

        public string NextId()
        {
            string? lastId = _specialistRefferals.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "specialistReferral1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("specialistReferral", ""));
                return $"specialistReferral{lastIdNumber + 1}";
            }
        }

    }
}
