using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service
{
    public class PrescriptionService
    {
        private IPrescriptionRepository _prescriptionRepository;

        public PrescriptionService(IPrescriptionRepository prescriptionRepository)
        {
            _prescriptionRepository = prescriptionRepository;
        }

        public List<Prescription> GetAll()
        {
            return _prescriptionRepository.GetAll();
        }

        public Prescription? GetById(string id)
        {
            return _prescriptionRepository.GetById(id);
        }

        public void Add(Prescription prescription)
        {
            _prescriptionRepository.Add(prescription);
        }

        public void Delete(Prescription prescription)
        {
            _prescriptionRepository.Delete(prescription);
        }

        public void Update(Prescription updatedPrescription)
        {
            _prescriptionRepository.Update(updatedPrescription);
        }

        public string NextId()
        {
            return _prescriptionRepository.NextId();
        }

        public List<Prescription> GetPatientsPrescriptions(string username)
        {
            return _prescriptionRepository.GetPatientsPrescriptions(username);
        }

        public List<Prescription> GetPrescriptionsEndingByTomorrow(string username)
        {
            DateTime tomorrow = DateTime.Now.AddDays(1).Date;

            return GetPatientsPrescriptions(username)
                .Where(prescription => prescription.TimeSlot.EndTime.Date <= tomorrow)
                .ToList();
        }

        public void UpdatePrescriptionTimeSlot(string id, DateTime newStartTime, DateTime newEndTime)
        {
            _prescriptionRepository.UpdatePrescriptionTimeSlot(id, newStartTime, newEndTime);
        }


    }
}
