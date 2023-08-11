using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;

namespace ZdravoCorp.SchedulingManagement.Appointments.Repository
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        Appointment? GetById(string id);
        void Add(Appointment appointment);
        void Delete(Appointment appointment);
        void Update(Appointment appointment);
        string NextId();
        List<Appointment> GetSearchAnamnesis(string keyword, Patient patient);
        List<Appointment> GetScheduledAppointmentsForPatient(Patient patient);
        List<Appointment> GetPatientFinishedAppointments(Patient patient);
        List<Appointment> GetDoctorAppointments(Doctor doctor);
        List<Appointment> GetAppointmentsWithinThreeDays(DateTime dateTime, Doctor doctor);
        void CancelAppointment(Appointment appointment);
        bool AreAppointmentsInTimeRangeForRoom(string roomId, TimeSlot timeSlot);
        void UpdateExamination(string id, string observation, List<string> symptoms, string selectedRoom);
        void AddPrescriptionId(Appointment appointment, string id);
        void UpdateAnamnesisSymptoms(string id, List<string> newSymptoms);
        Appointment? GetEarliestPatientAppointment(string patientUsername);
        List<Appointment> GetAppointmentsInNextTwoHours();
        int GetAppointmentDuration(AppointmentType appointmentType, string surgeryDuration);
        void ArrangeTakenAppointmentTerms(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor, List<Appointment> appointmentsForAllDoctors, Dictionary<string, string> potentialTakenTerms);
        void PostponeChoosenAppointment(Appointment postponedAppointment);
        bool IsAppointmentReadyForStart(Appointment appointment);
        List<Appointment> OrderByDate(ObservableCollection<Appointment>? appointments);
        List<Appointment> OrderByDoctor(ObservableCollection<Appointment>? appointments);
        List<Appointment> OrderBySpecialization(ObservableCollection<Appointment> appointments, DoctorService doctorService);
        void ChangeAppointmentRatedStatus(Appointment appointment);
    }
}
