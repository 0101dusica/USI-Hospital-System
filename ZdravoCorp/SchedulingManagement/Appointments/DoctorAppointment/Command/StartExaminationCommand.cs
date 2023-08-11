using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.View;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command
{
    public class StartExaminationCommand : BaseCommand
    {
        private DoctorAppointmentsViewModel _viewModel;
        private AppointmentService _service;

        public event EventHandler<bool> AppointmentStarted;

        public StartExaminationCommand(DoctorAppointmentsViewModel viewModel)
        {
            _viewModel = viewModel;
            _service = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedAppointment != null && _viewModel.SelectedAppointment.AppointmentStatus == AppointmentStatus.Scheduled && _service.IsAppointmentReadyForStart(_viewModel.SelectedAppointment);
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                var view = new DoctorExaminationView();
                view.DataContext = new DoctorExaminationViewModel(view, _viewModel.SelectedAppointment, _viewModel.LoggedDoctor);
                view.ShowDialog();
                AppointmentStarted?.Invoke(this, true);

                _viewModel.View.Close();
            }
            else { AppointmentStarted?.Invoke(this, false); }
        }
    }
}
