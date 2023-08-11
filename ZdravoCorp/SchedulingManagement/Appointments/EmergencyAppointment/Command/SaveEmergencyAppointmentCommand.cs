using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;
using ZdravoCorp.CommunicatonManagement.Notifications.Repository;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Service;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Command
{
    public class SaveEmergencyAppointmentCommand : BaseCommand
    {
        private PatientService _patientService;
        private DoctorService _doctorService;
        private EmergencyAppointmentService _emergencyappointmentService;
        private NotificationService _notificationService;
        private AddEmergencyAppointmentViewModel _viewModel;
        private Appointment _emergencyAppointment;
        private Appointment _postponedAppointment;


        public SaveEmergencyAppointmentCommand(AddEmergencyAppointmentViewModel addEmergencyAppointmentViewModel)
        {
            _viewModel = addEmergencyAppointmentViewModel;
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _postponedAppointment = new Appointment();
            _emergencyappointmentService = new EmergencyAppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _notificationService = new NotificationService(new NotificationRepository(new Serializer<Notification>()));
            _emergencyAppointment = new Appointment();
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
        }

        public override void Execute(object? parameter)
        {
            if (_viewModel.SelectedTerm != null)
            {
                string[] selectedTermInforamtions = _viewModel.SelectedTerm.Split(";");
                _viewModel.SpecializedDoctor = selectedTermInforamtions[0].Trim();
                string[] dateTime = selectedTermInforamtions[1].Split('T');
                _viewModel.StartDate = dateTime[0];
                _viewModel.StartTime = dateTime[1];
            }
            foreach (string appointmentId in _viewModel.potentialTakenTerms.Keys)
            {
                if (_viewModel.SelectedTerm == _viewModel.potentialTakenTerms[appointmentId])
                {
                    _postponedAppointment.Id = appointmentId;
                }
            }

            string dateAndTime;

            string doctorUsername = _viewModel.SpecializedDoctor;
            dateAndTime = _viewModel.StartDate + "T" + _viewModel.StartTime;
            DateTime startTime = DateTime.ParseExact(dateAndTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
           
            DateTime endTime = startTime.AddMinutes(_viewModel.emergencyAppointmentDuration);
            TimeSlot timeSlot = new TimeSlot();
            timeSlot.StartTime = startTime;
            timeSlot.EndTime = endTime;
            _emergencyAppointment = new Appointment(_emergencyappointmentService.NextId(), doctorUsername, _viewModel.SelectedPatient.Username, AppointmentType.EmergencyAppointment, AppointmentStatus.Scheduled, "", new Anamnesis(), timeSlot);
            
            _emergencyappointmentService.Add(_emergencyAppointment);
            _patientService.AddAppointmentId(_emergencyAppointment.Id, _patientService.GetByUsername(_viewModel.SelectedPatient.Username));
            _doctorService.AddAppointmentId(_emergencyAppointment.Id, _doctorService.GetByUsername(_viewModel.SpecializedDoctor));

            if (_viewModel.isAllTermsTaken)
            {
                _emergencyappointmentService.PostponeChoosenAppointment(_postponedAppointment);
                _notificationService.CreateDoctorNotificationAboutDelay(doctorUsername, _emergencyAppointment.Id, _viewModel.StartDate, _viewModel.StartTime);
                _notificationService.CreatePatientNotificationAboutDelay(_viewModel.SelectedPatient.Username, _emergencyAppointment.Id, _viewModel.StartDate, _viewModel.StartTime);

            }

            MessageBoxResult result = MessageBox.Show($"Successfully scheduled emergency appointment! \nReview information: \nAppointment Id: "
                    + _emergencyAppointment.Id + "\nDoctor: " + _viewModel.SpecializedDoctor + "\nPatient: " + _viewModel.SelectedPatient.Username
                    + "\nStart Date: " + _viewModel.StartDate + "\nStart Time: " + _viewModel.StartTime);

            if (result == MessageBoxResult.OK)
            {
                _viewModel.CreateEmergencyAppointmentVisibility = Visibility.Hidden;
                _viewModel.GetDataVisibility = Visibility.Collapsed;

            }
        }
    }
}
