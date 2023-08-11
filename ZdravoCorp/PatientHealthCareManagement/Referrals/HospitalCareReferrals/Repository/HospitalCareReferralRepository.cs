using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Repository
{
    public class HospitalCareReferralRepository : IHospitalCareReferralRepository
    {
        private static List<HospitalCareReferral> _hospitalCareReferrals = new List<HospitalCareReferral>();
        private const string _storagePath = "../../../Data/HospitalCareReferrals.json";

        private ISerializer<HospitalCareReferral> _serializer;


        public HospitalCareReferralRepository(ISerializer<HospitalCareReferral> serializer)
        {
            _serializer = serializer;
            _hospitalCareReferrals = Load();
        }

        public List<HospitalCareReferral> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _hospitalCareReferrals);
        }

        public List<HospitalCareReferral> GetAll()
        {
            return _hospitalCareReferrals;
        }

        public HospitalCareReferral? GetById(string id)
        {
            return _hospitalCareReferrals.SingleOrDefault(h => h.Id.Equals(id));
        }


        public void Add(HospitalCareReferral hospitalCareReferral)
        {
            _hospitalCareReferrals.Add(hospitalCareReferral);
            Save();
        }

        public void Delete(HospitalCareReferral hospitalCareReferral)
        {
            _hospitalCareReferrals.Remove(hospitalCareReferral);
            Save();
        }

        public void Update(HospitalCareReferral updatedReferral)
        {
            var existingRefferal = GetById(updatedReferral.Id);
            if (existingRefferal != null)
            {
                existingRefferal.PatientUsername = updatedReferral.PatientUsername;
                existingRefferal.AdditionalTests = updatedReferral.AdditionalTests;
                existingRefferal.Therapy = updatedReferral.Therapy;
                existingRefferal.TimeSlot = updatedReferral.TimeSlot;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _hospitalCareReferrals.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "hospitalReferral1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("hospitalReferral", ""));
                return $"hospitalReferral{lastIdNumber + 1}";
            }
        }

        
    }
}
