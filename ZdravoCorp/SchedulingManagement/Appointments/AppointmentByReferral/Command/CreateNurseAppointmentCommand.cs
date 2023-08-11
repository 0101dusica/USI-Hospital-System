using System;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.Command
{
    public class CreateNurseAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private DoctorService _doctorService;
        private PatientService _patientService;
        private AppointmentDoctorAvailabilityService _appointmentDoctorAvailabilityService;
        private AppointmentPatientAvailabilityService _appointmentPatientAvailabilityService;
        private NurseCreateAppointmentViewModel _viewModel;
        private int _appointmentDuration;
        private Appointment _newAppointment;

        public CreateNurseAppointmentCommand(NurseCreateAppointmentViewModel viewModel)
        {
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _appointmentDoctorAvailabilityService = new AppointmentDoctorAvailabilityService(_doctorService);
            _appointmentPatientAvailabilityService = new AppointmentPatientAvailabilityService();
            _viewModel = viewModel;
            _newAppointment = new Appointment();
            _appointmentDuration = 0;
        }

        private bool CanExecute(object parameter)
        {
            if (_viewModel.isReferralSelected() && _viewModel.isDatePassed() && _viewModel.isSugeryDurationIsValid())
            {

                _appointmentDuration = _appointmentService.GetAppointmentDuration(_viewModel.SelectedAppointmentType, _viewModel.SurgeryDuration);
                DateTime startDateTime = _viewModel.SelectedDateTime;
                DateTime endDateTime = startDateTime.AddMinutes(_appointmentDuration);

                string toDoctorUsername = _viewModel.SelectedReferral.ToDoctor;
                Doctor? doctor = _doctorService.GetByUsername(toDoctorUsername);
                bool isDoctorAvailable = true;
                bool isPatientAvailable = true;

                try
                {
                    _appointmentDoctorAvailabilityService.CheckDoctorAvailability(startDateTime, endDateTime, doctor, _appointmentService);
                    isDoctorAvailable = true;
                }
                catch (Exception ex)
                {
                    isDoctorAvailable = false;
                }

                _newAppointment = new Appointment(_appointmentService.NextId(), toDoctorUsername, _viewModel.SelectedPatient.Username, _viewModel.SelectedAppointmentType, AppointmentStatus.Scheduled, "", new Anamnesis(), new TimeSlot(startDateTime, endDateTime));

                try
                {
                    _appointmentPatientAvailabilityService.IsPatientAvailable(_newAppointment, _viewModel.SelectedPatient, _appointmentService);
                    isPatientAvailable = true;
                }
                catch (Exception ex)
                {
                    isPatientAvailable = false;
                }

                if (isDoctorAvailable && isPatientAvailable)
                {
                    return true;
                }
                else if (!isDoctorAvailable && isPatientAvailable)
                {
                    MessageBox.Show($"Doctor is not available in choosen term.\nPlease choose another!");
                    return false;
                }
                else if (!isPatientAvailable && isDoctorAvailable)
                {
                    MessageBox.Show($"Patient is not available in choosen term.\nPlease choose another!");
                    return false;
                }
                else
                {
                    MessageBox.Show($"Doctor and patient are not available in choosen term.\nPlease choose another!");
                    return false;
                }

            }
            else
            {
                return false;
            }

        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _appointmentService.Add(_newAppointment);
                _patientService.AddAppointmentId(_newAppointment.Id, _patientService.GetByUsername(_viewModel.SelectedPatient.Username));
                _doctorService.AddAppointmentId(_newAppointment.Id, _doctorService.GetByUsername(_viewModel.SelectedReferral.ToDoctor));
                MessageBox.Show($"Successfully scheduled appointment as per referral!");
            }
        }
    }
}
