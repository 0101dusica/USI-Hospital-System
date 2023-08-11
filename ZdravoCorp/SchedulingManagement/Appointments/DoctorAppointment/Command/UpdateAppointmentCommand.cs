using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command
{
    public class UpdateAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;

        private UpdateAppointmentViewModel _viewModel;

        public event EventHandler<bool> AppointmentUpdated;

        public UpdateAppointmentCommand(UpdateAppointmentViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
        }

        private bool CanExecute(object parameter)
        {
            return  _viewModel.SelectedDateTime > DateTime.Now && _viewModel.Duration>0;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    var selectedAppointment = _viewModel.SelectedAppointment;
                    var doctor = _appointmentService.GetDoctorByUsername(selectedAppointment.DoctorUsername);
                    var patient = _appointmentService.GetPatientByUsername(selectedAppointment.PatientUsername);

                    var updatedAppointment = new Appointment(selectedAppointment.Id, selectedAppointment.DoctorUsername, selectedAppointment.PatientUsername, selectedAppointment.AppointmentType, new TimeSlot(_viewModel.SelectedDateTime, _viewModel.SelectedDateTime.AddMinutes(_viewModel.Duration)));
                    _appointmentService.TryUpdate(_viewModel.SelectedAppointment!, doctor, doctor,
                            _viewModel.SelectedDateTime, _viewModel.SelectedDateTime.AddMinutes(_viewModel.Duration), patient);
                    AppointmentUpdated?.Invoke(this, true);

                    _viewModel.View.Close();
                }
                catch (Exception exception)
                {
                    AppointmentUpdated?.Invoke(this, false);
                }

            }
            else { AppointmentUpdated?.Invoke(this, false); }
        }
    }
}

