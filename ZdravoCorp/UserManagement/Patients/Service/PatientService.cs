using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;

namespace ZdravoCorp.UserManagement.Patients.Service
{
    public class PatientService
    {
        private IPatientRepository _patientRepository;
        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public List<Patient> GetAll()
        {
            return _patientRepository.GetAll();
        }

        public Patient? GetByUsername(string username)
        {
            return _patientRepository.GetByUsername(username);
        }

        public void Add(Patient patient)
        {
            _patientRepository.Add(patient);
        }

        public void Delete(Patient patient)
        {
            _patientRepository.Delete(patient);
        }

        public void Update(string username, string firstName, string lastName, string password, UserStatus userStatus, MedicalRecord medicalRecord)
        {
            _patientRepository.Update(username, firstName, lastName, password, userStatus, medicalRecord);
        }

        public List<Patient> GetMatchingPatients(string searchText)
        {
            return _patientRepository.GetMatchingPatients(searchText);
        }


        internal void AddAppointmentId(string id, Patient patient)
        {
            _patientRepository.AddAppointmentId(id, patient);
        }


        public void UpdateMedicalRecordAllergies(string username, List<string> newAllergies)
        {
            _patientRepository.UpdateMedicalRecordAllergies(username, newAllergies);
        }

        public void UpdateMedicalRecordMedicalHistory(string username, List<string> newMedicalHistory)
        {
            _patientRepository.UpdateMedicalRecordMedicalHistory(username, newMedicalHistory);
        }
    }
}
