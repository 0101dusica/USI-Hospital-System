using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private static List<Prescription> _prescriptions = new List<Prescription>();
        private const string _storagePath = "../../../Data/Prescriptions.json";

        private ISerializer<Prescription> _serializer;

        public PrescriptionRepository(ISerializer<Prescription> serializer)
        {
            _serializer = serializer;
            _prescriptions = Load();
        }

        public List<Prescription> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _prescriptions);
        }

        public List<Prescription> GetAll()
        {
            return _prescriptions;
        }

        public Prescription? GetById(string id)
        {
            return _prescriptions.FirstOrDefault(p => p.Id == id);
        }

        public void Add(Prescription prescription)
        {
            _prescriptions.Add(prescription);
            Save();
        }

        public void Delete(Prescription hospitalCare)
        {
            _prescriptions.Remove(hospitalCare);
            Save();
        }


        public void Update(Prescription prescription)
        {
            var existingPrescription = GetById(prescription.Id);
            if (existingPrescription != null)
            {
                existingPrescription.Instruction = prescription.Instruction;
                existingPrescription.MedicineId = prescription.MedicineId;
                existingPrescription.DailyUsage = prescription.DailyUsage;
                existingPrescription.TimeSet = prescription.TimeSet;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _prescriptions.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "prescription1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("prescription", ""));
                return $"prescription{lastIdNumber + 1}";
            }
        }

        public List<Prescription> GetPatientsPrescriptions(string username)
        {
            return GetAll().Where(prescription => prescription.PatientUsername.Equals(username)).ToList();
        }

        public void UpdatePrescriptionTimeSlot(string id, DateTime newStartTime, DateTime newEndTime)
        {
            Prescription? prescription = GetById(id);
            if (prescription != null)
            {
                prescription.TimeSlot = new TimeSlot(newStartTime, newEndTime);
            }
            Save();
        }


    }
}
