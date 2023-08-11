using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command
{
    public class AddAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;

        private CreateAppointmentViewModel _viewModel;

        public event EventHandler<bool> AppointmentAdded;

        public AddAppointmentCommand(CreateAppointmentViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedPatient != null && _viewModel.SelectedDateTime > DateTime.Now && _viewModel.Duration>0;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    var appointment = new Appointment(_appointmentService.NextId(), _viewModel.LoggedDoctor.Username, _viewModel.SelectedPatient.Username, _viewModel.SelectedAppointmentType,new TimeSlot(_viewModel.SelectedDateTime, _viewModel.SelectedDateTime.AddMinutes(_viewModel.Duration)));
                    _appointmentService.CreateAppointmentDoctor(appointment);
                    AppointmentAdded?.Invoke(this, true);
                }
                catch (Exception exception) 
                {
                    AppointmentAdded?.Invoke(this, false);
                }
                
            }
            else { AppointmentAdded?.Invoke(this, false); }
        }
    }
}
