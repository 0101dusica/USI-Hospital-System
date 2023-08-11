using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Control.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Control.Command
{
    public class AddControlCommand : BaseCommand
    {
        private AppointmentService _appointmentService;

        private AddControlViewModel _viewModel;

        public event EventHandler<bool> ControlAdded;

        public AddControlCommand(AddControlViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));

        }

        private bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                try
                {
                    DateTime startTime = _viewModel.Date.Date + _viewModel.Time;
                    var appointment = new Appointment(_appointmentService.NextId(), _viewModel.LoggedDoctor.Username, _viewModel.SelectedHospitalCare.PatientUsername, _viewModel.SelectedAppointmentType, new TimeSlot(startTime, startTime.AddMinutes(_viewModel.Duration)));
                    _appointmentService.CreateAppointmentDoctor(appointment);
                    ControlAdded?.Invoke(this, true);

                    _viewModel.View.Close();
                }
                catch (Exception exception)
                {
                    ControlAdded?.Invoke(this, false);
                }

            }
            else { ControlAdded?.Invoke(this, false); }
        }
    }
}
