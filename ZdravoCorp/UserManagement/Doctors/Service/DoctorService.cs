using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;

namespace ZdravoCorp.UserManagement.Doctors.Service
{
    public class DoctorService
    {
        private IDoctorRepository _doctorRepository;
        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public List<Doctor> GetAll()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor? GetByUsername(string username)
        {
            return _doctorRepository.GetByUsername(username);
        }

        public void Add(Doctor doctor)
        {
            _doctorRepository.Add(doctor);
        }

        public void Delete(Doctor doctor)
        {
            _doctorRepository.Delete(doctor);
        }

        public void Update(Doctor updatedDoctor)
        {
            _doctorRepository.Update(updatedDoctor);
        }

        public void AddAppointmentId(string id, Doctor selectedDoctor)
        {
            _doctorRepository.AddAppointmentId(id, selectedDoctor);
        }

        public void AddFreeDaysId(string id, Doctor selectedDoctor)
        {
            _doctorRepository.AddFreeDaysId(id, selectedDoctor);
        }

        public string GetDoctorSpecialization(string doctorUsername)
        {
            Doctor? doctor = GetByUsername(doctorUsername);
            string specialization = doctor!.Specialization.ToString();
            return specialization;
        }

        public List<Doctor> OrderBySpecialization(ObservableCollection<Doctor> doctors)
        {
            return _doctorRepository.OrderBySpecialization(doctors);
        }

        public List<Doctor> OrderByAverageGrade(ObservableCollection<Doctor> doctors)
        {
            return _doctorRepository.OrderByAverageGrade(doctors);
        }

        public List<Doctor> OrderByLastName(ObservableCollection<Doctor> doctors)
        {
            return _doctorRepository.OrderByLastName(doctors);
        }

        public List<Doctor> OrderByName(ObservableCollection<Doctor> doctors)
        {
            return _doctorRepository.OrderByName(doctors);
        }

        internal IEnumerable<Doctor> GetSearchName(string keywordName)
        {
            return _doctorRepository.GetSearchName(keywordName);
        }

        internal IEnumerable<Doctor> GetSearchLastName(string keywordLastName)
        {
            return _doctorRepository.GetSearchLastName(keywordLastName);
        }

        internal IEnumerable<Doctor> GetSearchSpecialization(string keywordSpecialization)
        {
            return _doctorRepository.GetSearchSpecialization(keywordSpecialization);
        }
        public List<Doctor> GetWithoutLoggedDoctor(Doctor _loggedDoctor)
        {
            return _doctorRepository.GetWithoutLoggedDoctor(_loggedDoctor);
        }

        public List<string> GetAllSpecializations()
        {
            return _doctorRepository.GetAllSpecializations();
        }

        public List<Doctor> GetDoctorWithoutAppointments()
        {
            return _doctorRepository.GetDoctorWithoutAppointments();
        }

        public DateTime FindDoctorsEarliestTerm(Dictionary<Doctor, DateTime> earliestTermForEachDoctor, string username)
        {
            return _doctorRepository.FindDoctorsEarliestTerm(earliestTermForEachDoctor, username);
        }

        public Doctor? FindDoctorWithEarliestTerm(Dictionary<Doctor, DateTime> earliestTermForEachDoctor)
        {
            if (earliestTermForEachDoctor.Count != 0)
            {
                DateTime earliestTerm = earliestTermForEachDoctor.Values.Min();
                return earliestTermForEachDoctor.FirstOrDefault(pair => pair.Value == earliestTerm).Key;
            }
            return null;
        }

        public List<Doctor> FilterBySpecialization(string specialization)
        {
            List<Doctor> doctors = _doctorRepository.GetAll();
            return doctors.Where(doctor => doctor.Specialization.ToString().Equals(specialization)).ToList();
        }

        public List<Doctor> GetGeneralPractitioners()
        {
            return _doctorRepository.GetAll()
                .Where(doctor => doctor.Specialization.Equals(Specialization.GeneralPractitioner))
                .ToList();
        }

        public void RemoveAppointmentId(string id, Doctor doctor)
        {
            _doctorRepository.RemoveAppointmentId(id, doctor);
        }

        public bool IsDoctorWithSpecializationExist(Doctor doctor, Specialization specialization)
        {
            return _doctorRepository.IsDoctorWithSpecializationExist(doctor, specialization);
        }

        public void UpdateDoctorGrades(Doctor doctor, List<int> newGrades)
        {
           
            _doctorRepository.UpdateDoctorGrades(doctor, newGrades);
        }

        public ObservableCollection<Doctor> GetAllNursesExceptLoggedIn(Doctor doctor)
        {
            return _doctorRepository.GetAllNursesExceptLoggedIn(doctor);
        }
    }
}
