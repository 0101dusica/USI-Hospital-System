using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Doctors.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private static List<Doctor> _doctors = new List<Doctor>();
        private const string _storagePath = "../../../../Data/Doctors.json";

        private ISerializer<Doctor> _serializer;

        public DoctorRepository(ISerializer<Doctor> serializer)
        {
            _serializer = serializer;
            _doctors = Load();
        }

        public List<Doctor> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _doctors);
        }

        public List<Doctor> GetAll()
        {
            return _doctors;
        }

        public Doctor? GetByUsername(string username)
        {
            return _doctors.FirstOrDefault(d => d.Username.Equals(username));
        }

        public void Add(Doctor doctor)
        {
            _doctors.Add(doctor);
            Save();
        }

        public void Delete(Doctor doctor)
        {
            _doctors.Remove(doctor);
            Save();
        }


        public void Update(Doctor updatedDoctor)
        {
            var existingDoctor = GetByUsername(updatedDoctor.Username);
            if (existingDoctor != null)
            {
                existingDoctor.FirstName = updatedDoctor.FirstName;
                existingDoctor.LastName = updatedDoctor.LastName;
                existingDoctor.Password = updatedDoctor.Password;
                existingDoctor.UserStatus = updatedDoctor.UserStatus;
                existingDoctor.Specialization = updatedDoctor.Specialization;

                Save();
            }
        }
        public List<Doctor> GetWithoutLoggedDoctor(Doctor loggedDoctor)
        {
            return GetAll().Where(d => d.Username != loggedDoctor.Username).ToList();
        }

        public List<string> GetAllSpecializations()
        {
            return GetAll().Select(doctor => doctor.Specialization.ToString()).Distinct().ToList();
        }

        public List<Doctor> GetDoctorWithoutAppointments()
        {
            return _doctors.Where(doctor => doctor.AppointmentIds.Count == 0).ToList();
        }


        public void AddAppointmentId(string id, Doctor selectedDoctor)
        {
            selectedDoctor.AppointmentIds.Add(id);
            Save();
        }

        public void AddFreeDaysId(string id, Doctor selectedDoctor)
        {
            selectedDoctor.FreeDaysIds.Add(id);
            Save();
        }
        public List<Doctor> OrderBySpecialization(ObservableCollection<Doctor> doctors)
        {
            return new List<Doctor>(doctors.OrderBy(doctor => doctor.Specialization.ToString()));
        }

        public List<Doctor> OrderByName(ObservableCollection<Doctor> doctors)
        {
            return new List<Doctor>(doctors.OrderBy(doctor => doctor.FirstName));
        }

        public List<Doctor> OrderByLastName(ObservableCollection<Doctor> doctors)
        {
            return new List<Doctor>(doctors.OrderBy(doctor => doctor.LastName));
        }
        public List<Doctor> OrderByAverageGrade(ObservableCollection<Doctor> doctors)
        {
            return new List<Doctor>(doctors.OrderBy(doctor => doctor.AverageGrade));
        }

        public IEnumerable<Doctor> GetSearchName(string keywordName)
        {
            return GetAll().Where(doctor => doctor.FirstName.Contains(keywordName));
        }

        public IEnumerable<Doctor> GetSearchLastName(string keywordLastName)
        {
            return GetAll().Where(doctor => doctor.LastName.Contains(keywordLastName));
        }

        public IEnumerable<Doctor> GetSearchSpecialization(string keywordSpecialization)
        {
            return GetAll().Where(doctor => doctor.Specialization.ToString().Contains(keywordSpecialization));
        }

        public DateTime FindDoctorsEarliestTerm(Dictionary<Doctor, DateTime> earliestTermForEachDoctor, string username)
        {
            foreach (Doctor doctor in earliestTermForEachDoctor.Keys)
            {
                if (doctor.Username.Equals(username))
                {
                    return earliestTermForEachDoctor[doctor];
                }
            }

            return DateTime.MinValue;
        }

        public void RemoveAppointmentId(string id, Doctor doctor)
        {
            doctor.AppointmentIds.Remove(id);
            Save();
        }

        public bool IsDoctorWithSpecializationExist(Doctor doctor, Specialization specialization)
        {
            return GetWithoutLoggedDoctor(doctor).Any(doctor => doctor.Specialization == specialization);
        }

        public void UpdateDoctorGrades(Doctor doctor, List<int> newGrades)
        {
            foreach (var grade in newGrades)
            {
                doctor.Grades.Add(grade);
            }
            Save();

        }

        public ObservableCollection<Doctor> GetAllNursesExceptLoggedIn(Doctor loggedDoctor)
        {
            ObservableCollection<Doctor> doctors = new ObservableCollection<Doctor>();
            foreach (var doctor in _doctors)
            {
                if (!(doctor.Username.Equals(loggedDoctor.Username))) doctors.Add(doctor);;
            }
            return doctors;
        }
    }
}

