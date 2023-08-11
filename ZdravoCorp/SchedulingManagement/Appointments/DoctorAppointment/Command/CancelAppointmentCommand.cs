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
    public class CancelAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;

        private DoctorAppointmentsViewModel _viewModel;

        public event EventHandler<bool> AppointmentCancelled;

        public CancelAppointmentCommand(DoctorAppointmentsViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedAppointment != null && _viewModel.SelectedAppointment.AppointmentStatus == AppointmentStatus.Scheduled;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _appointmentService.CancelAppointment(_viewModel.SelectedAppointment);
                AppointmentCancelled?.Invoke(this, true);
            }
        }
    }
}

