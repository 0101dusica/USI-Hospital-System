using System;
using System.Collections.Generic;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository
{
    public interface IPrescriptionRepository
    {
        List<Prescription> GetAll();
        Prescription? GetById(string id);
        void Add(Prescription prescription);
        void Delete(Prescription prescription);
        void Update(Prescription prescription);
        string NextId();
        List<Prescription> GetPatientsPrescriptions(string username);
        void UpdatePrescriptionTimeSlot(string id, DateTime newStartTime, DateTime newEndTime);
    }
}
