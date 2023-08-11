using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoCorp.UserManagement.Doctors.Model;

namespace ZdravoCorp.UserManagement.Doctors.Repository
{
    public interface IDoctorRepository
    {
        List<Doctor> GetAll();
        Doctor? GetByUsername(string username);
        void Add(Doctor doctor);
        void Delete(Doctor doctor);
        void Update(Doctor updatedDoctor);
        List<Doctor> GetWithoutLoggedDoctor(Doctor loggedDoctor);
        List<string> GetAllSpecializations();
        List<Doctor> GetDoctorWithoutAppointments();
        void AddAppointmentId(string id, Doctor selectedDoctor);
        void AddFreeDaysId(string id, Doctor selectedDoctor);
        List<Doctor> OrderBySpecialization(ObservableCollection<Doctor> doctors);
        List<Doctor> OrderByName(ObservableCollection<Doctor> doctors);
        List<Doctor> OrderByLastName(ObservableCollection<Doctor> doctors);
        List<Doctor> OrderByAverageGrade(ObservableCollection<Doctor> doctors);
        IEnumerable<Doctor> GetSearchName(string keywordName);
        IEnumerable<Doctor> GetSearchLastName(string keywordLastName);
        IEnumerable<Doctor> GetSearchSpecialization(string keywordSpecialization);
        DateTime FindDoctorsEarliestTerm(Dictionary<Doctor, DateTime> earliestTermForEachDoctor, string username);
        void RemoveAppointmentId(string id, Doctor doctor);
        bool IsDoctorWithSpecializationExist(Doctor doctor, Specialization specialization);
        void UpdateDoctorGrades(Doctor doctor, List<int> newGrades);
        ObservableCollection<Doctor> GetAllNursesExceptLoggedIn(Doctor doctor);
    }
}
