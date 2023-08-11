using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.View;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command
{
    public class ShowUpdateAppointmentCommand : BaseCommand
    {
        private AppointmentService _appointmentService;

        private DoctorAppointmentsViewModel _viewModel;

        private readonly Action<bool> _resultCallback;
        public ShowUpdateAppointmentCommand(DoctorAppointmentsViewModel viewModel, Action<bool> resultCallback)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));//pormeniti
            _resultCallback = resultCallback;
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedAppointment != null && _viewModel.SelectedAppointment.AppointmentStatus== AppointmentStatus.Scheduled;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Window view = new UpdateAppointmentView();
                view.DataContext = new UpdateAppointmentViewModel(view, _viewModel);
                view.ShowDialog();
                _resultCallback?.Invoke(true);
            }
            else
            {
                _resultCallback?.Invoke(false);
            }
        }
    }
}
