using System;
using ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.View;
using ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Doctors.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands
{
    public class ShowPatientAppointmentsCommand : BaseCommand
    {
        private Patient _patient;
        private DoctorService _doctorService;
        private AppointmentService _appointmentService;
        private PatientService _patientService;
        private PatientAppointmentActionsService _patientAppointmentActionsService;
        private PatientSearchDoctorsViewModel _patientSearchDoctorsViewModel;

        public ShowPatientAppointmentsCommand(PatientSearchDoctorsViewModel patientSearchDoctorsViewModel, Patient patient, DoctorService doctorService, AppointmentService appointmentService, PatientService patientService, PatientAppointmentActionsService patientAppointmentActionsService)
        {
            _patientSearchDoctorsViewModel = patientSearchDoctorsViewModel;
            _patient = patient;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _patientService = patientService;
            _patientAppointmentActionsService = patientAppointmentActionsService;
        }
        public override void Execute(object? parameter)
        {
            PatientAppointmentsView patientAppointmentsView = new PatientAppointmentsView();
            patientAppointmentsView.DataContext = new PatientAppointmentsViewModel(_patientSearchDoctorsViewModel, _patient, _doctorService, _appointmentService, _patientService, _patientAppointmentActionsService);
            patientAppointmentsView.ShowDialog();
        }
    }
}