using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository
{
    public class HospitalCareRepository : IHospitalCareRepository
    {
        private static List<HospitalCare> _hospitalCares = new List<HospitalCare>();
        private const string _storagePath = "../../../Data/HospitalCares.json";

        private ISerializer<HospitalCare> _serializer;

        public HospitalCareRepository(ISerializer<HospitalCare> serializer)
        {
            _serializer = serializer;
            _hospitalCares = Load();
        }

        public List<HospitalCare> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _hospitalCares);
        }

        public List<HospitalCare> GetAll()
        {
            return _hospitalCares;
        }

        public HospitalCare? GetById(string id)
        {
            return _hospitalCares.FirstOrDefault(h => h.Id.Equals(id));
        }

        public void Add(HospitalCare hospitalCare)
        {
            _hospitalCares.Add(hospitalCare);
            Save();
        }

        public void Delete(HospitalCare hospitalCare)
        {
            _hospitalCares.Remove(hospitalCare);
            Save();
        }


        public void Update(HospitalCare hospitalCare)
        {
            var existingCare = GetById(hospitalCare.Id);
            if (existingCare != null)
            {
                existingCare.Therapy = hospitalCare.Therapy;
                existingCare.TimeSlot = hospitalCare.TimeSlot;
                existingCare.HospitalCareStatus = hospitalCare.HospitalCareStatus;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _hospitalCares.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "hospitalCare1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("hospitalCare", ""));
                return $"hospitalCare{lastIdNumber + 1}";
            }
        }

        public List<HospitalCare> GetHospitalCaresForPatient(string patientUsername)
        {
            return GetAll().Where(hc => hc.PatientUsername == patientUsername).ToList();
        }
    }
}
