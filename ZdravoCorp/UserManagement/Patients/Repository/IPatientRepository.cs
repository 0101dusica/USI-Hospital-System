using System.Collections.Generic;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.UserManagement.Patients.Model;

namespace ZdravoCorp.UserManagement.Patients.Repository
{
    public interface IPatientRepository
    {
        List<Patient> GetAll();
        Patient? GetByUsername(string username);
        void Add(Patient patient);
        void Delete(Patient patient);
        void Update(string username, string firstName, string lastName, string password, UserStatus userStatus, MedicalRecord medicalRecord);
        void UpdateMedicalRecordAllergies(string username, List<string> newAllergies);
        void UpdateMedicalRecordMedicalHistory(string username, List<string> newMedicalHistory);
        List<Patient> GetMatchingPatients(string searchText);
        void AddAppointmentId(string id, Patient patient);
        void UpdatePatientsAppointments(string username, string appointmentId);
    }
}
