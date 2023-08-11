using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Service
{
    public class AppointmentService
    {
        private IAppointmentRepository _appointmentRepository;
        private AppointmentPatientAvailabilityService _appointmentPatientAvailabilityService;
        private AppointmentDoctorAvailabilityService _appointmentDoctorAvailabilityService;
        private DoctorService _doctorService;
        private PatientService _patientService;
        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentPatientAvailabilityService = new AppointmentPatientAvailabilityService();
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _appointmentDoctorAvailabilityService = new AppointmentDoctorAvailabilityService(_doctorService);

        }
        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public Appointment? GetById(string id)
        {
            return _appointmentRepository.GetById(id);
        }

        public void Add(Appointment appointment)
        {
            _appointmentRepository.Add(appointment);
        }

        public void Delete(Appointment appointment)
        {
            _appointmentRepository.Delete(appointment);
        }

        public void Update(Appointment updatedAppointment)
        {
            _appointmentRepository.Update(updatedAppointment);
        }

        public string NextId()
        {
            return _appointmentRepository.NextId();
        }

        public void CreateAppointment(DoctorService doctorService, PatientService patientService, Appointment appointment)
        {
            Doctor? doctor = _doctorService.GetByUsername(appointment.DoctorUsername);
            Patient? patient = patientService.GetByUsername(appointment.PatientUsername);
            CheckAppointmentAvailability(appointment, patient, doctor!);
            Add(appointment);
            //patient.MedicalRecord.AppointmentIds.Add(appointment.Id);
            patientService.AddAppointmentId(appointment.Id, patient);
            doctorService.AddAppointmentId(appointment.Id, doctor!);
        }

        public void CreateAppointmentDoctor(Appointment appointment) // ja bih novi service napravila
        {
            Doctor? doctor = _doctorService.GetByUsername(appointment.DoctorUsername);
            Patient? patient = _patientService.GetByUsername(appointment.PatientUsername);
            CheckAppointmentAvailability(appointment, patient, doctor);
            Add(appointment);
            _patientService.AddAppointmentId(appointment.Id, patient);
            _doctorService.AddAppointmentId(appointment.Id, doctor);
        }

        public void CheckAppointmentAvailability(Appointment appointment, Patient patient, Doctor doctor)
        {
            _appointmentPatientAvailabilityService.IsPatientAvailable(appointment, patient, this);
            _appointmentDoctorAvailabilityService.CheckDoctorAvailability(appointment.TimeSlot.StartTime, appointment.TimeSlot.EndTime, doctor!, this);
        }
        public List<Appointment> GetPatientFinishedAppointments(Patient patient)
        {
            return _appointmentRepository.GetPatientFinishedAppointments(patient);
        }

        public List<Appointment> GetScheduledAppointmentsForPatient(Patient patient)
        {
            return _appointmentRepository.GetScheduledAppointmentsForPatient(patient);
        }

        public List<Appointment> OrderByDoctor(ObservableCollection<Appointment>? appointments)
        {
            return _appointmentRepository.OrderByDoctor(appointments);
        }
        public List<Appointment> OrderByDate(ObservableCollection<Appointment>? appointments)
        {
            return _appointmentRepository.OrderByDate(appointments);
        }
        public List<Appointment> OrderBySpecialization(ObservableCollection<Appointment> appointments,
            DoctorService doctorService)
        {
            return _appointmentRepository.OrderBySpecialization(appointments, doctorService);
        }
        public List<Appointment> GetSearchAnamnesis(string keyword, Patient loggedPatient)
        {
            return _appointmentRepository.GetSearchAnamnesis(keyword, loggedPatient);
        }

        public void TryUpdate(Appointment appointment, Doctor doctor, Doctor selecterdDoctor, DateTime startTime, DateTime endTime, Patient patient)
        {
            _appointmentDoctorAvailabilityService.CheckIsDoctorUpdatedOrAvailable(doctor, selecterdDoctor, appointment, startTime, endTime, this);
            Appointment updatedAppointment = new Appointment(appointment.Id, selecterdDoctor.Username,
                appointment.PatientUsername, appointment.AppointmentType, appointment.AppointmentStatus,
                appointment.RoomId, appointment.Anamnesis, new TimeSlot(startTime, endTime));
            _appointmentPatientAvailabilityService.IsPatientAvailable(updatedAppointment, patient, this);
            Update(updatedAppointment);
        }

        public List<Appointment> GetDoctorAppointments(Doctor doctor)
        {
            return _appointmentRepository.GetDoctorAppointments(doctor);
        }

        public List<Appointment> GetAppointmentsWithinThreeDays(DateTime dateTime, Doctor doctor)
        {
            return _appointmentRepository.GetAppointmentsWithinThreeDays(dateTime, doctor);
        }

        public void CancelAppointment(Appointment appointment)
        {
            _appointmentRepository.CancelAppointment(appointment);
        }

        public void UpdateExamination(string id, string observation, List<string> symptoms, string selectedRoom)
        {
            _appointmentRepository.UpdateExamination(id, observation, symptoms, selectedRoom);
        }

        public void AddPrescriptionId(Appointment appointment, string id)
        {
            _appointmentRepository.AddPrescriptionId(appointment, id);
            //return _appointmentRepository.NextId();
        }

        //Nurse function
        public void UpdateAnamnesisSymptoms(string id, List<string> newAnamnesis)
        {
            _appointmentRepository.UpdateAnamnesisSymptoms(id, newAnamnesis);
        }
        //Nurse function
        public Appointment? GetEarliestPatientAppointment(string patientUsername)
        {
            return _appointmentRepository.GetEarliestPatientAppointment(patientUsername);
        }
        //Nurse function
        public List<Appointment> GetAppointmentsInNextTwoHours()
        {
            return _appointmentRepository.GetAppointmentsInNextTwoHours();
        }
        //Nurse function
        public void PostponeChoosenAppointment(Appointment postponedAppointment)
        {
            _appointmentRepository.PostponeChoosenAppointment(postponedAppointment);
        }
        public bool IsAppointmentReadyForStart(Appointment appointment)
        {
            return _appointmentRepository.IsAppointmentReadyForStart(appointment);
        }
        //Nurse function
        public int GetAppointmentDuration(AppointmentType appointmentType, string surgeryDuration)
        {
            return _appointmentRepository.GetAppointmentDuration(appointmentType, surgeryDuration);
        }

        public Doctor GetDoctorByUsername(string doctorUsername)
        {
            return _doctorService.GetByUsername(doctorUsername);
        }
        public Patient GetPatientByUsername(string patientUsername)
        {
            return _patientService.GetByUsername(patientUsername);
        }

        public void ChangeAppointmentRatedStatus(Appointment appointment)
        {
          _appointmentRepository.ChangeAppointmentRatedStatus(appointment);
        }

    }
}

