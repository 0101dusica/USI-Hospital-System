using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Patients.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private static List<Patient> _patients = new List<Patient>();
        private const string _storagePath = "../../../../Data/Patients.json";

        private ISerializer<Patient> _serializer;


        public PatientRepository(ISerializer<Patient> serializer)
        {
            _serializer = serializer;
            _patients = Load();
        }

        public List<Patient> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _patients);
        }

        public List<Patient> GetAll()
        {
            return _patients;
        }

        public Patient? GetByUsername(string username)
        {
            return _patients.FirstOrDefault(p => p.Username.Equals(username));
        }

        public void Add(Patient patient)
        {
            _patients.Add(patient);
            Save();
        }

        public void Delete(Patient patient)
        {
            _patients.Remove(patient);
            Save();
        }

        public void Update(string username, string firstName, string lastName, string password, UserStatus userStatus, MedicalRecord medicalRecord)
        {
            var existingPatient = GetByUsername(username);
            if (existingPatient != null)
            {
                existingPatient.FirstName = firstName;
                existingPatient.LastName = lastName;
                existingPatient.Password = password;
                existingPatient.UserStatus = userStatus;
                existingPatient.MedicalRecord.Height = medicalRecord.Height;
                existingPatient.MedicalRecord.Weight = medicalRecord.Weight;
                existingPatient.MedicalRecord.MedicalHistory = medicalRecord.MedicalHistory;
                existingPatient.MedicalRecord.Allergies = medicalRecord.Allergies;

                Save();
            }
        }

        public void UpdateMedicalRecordAllergies(string username, List<string> newAllergies)
        {
            var existingPatient = GetByUsername(username);
            if (existingPatient != null)
            {
                existingPatient.MedicalRecord.Allergies = newAllergies;
            }
            Save();
        }

        public void UpdateMedicalRecordMedicalHistory(string username, List<string> newMedicalHistory)
        {
            var existingPatient = GetByUsername(username);
            if (existingPatient != null)
            {
                existingPatient.MedicalRecord.MedicalHistory = newMedicalHistory;
            }
            Save();
        }

        public List<Patient> GetMatchingPatients(string searchText)
        {
            return GetAll().Where(p => p.FirstName.Contains(searchText) || p.LastName.Contains(searchText) || p.Username.Contains(searchText) || p.UserStatus.ToString().Contains(searchText)).ToList();
        }

        public void AddAppointmentId(string id, Patient patient)
        {
            patient.MedicalRecord.AppointmentIds.Add(id);
            Save();
        }

        public void UpdatePatientsAppointments(string username, string appointmentId)
        {
            Patient patient = GetByUsername(username);
            if (patient != null)
            {
                patient.MedicalRecord.AppointmentIds.Add(appointmentId);
            }
            Save();
        }
    }
}
