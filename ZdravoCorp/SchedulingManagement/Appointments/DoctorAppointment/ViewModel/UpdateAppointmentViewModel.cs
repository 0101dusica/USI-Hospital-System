using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel

{
    public class UpdateAppointmentViewModel : BaseViewModel
    {
        public Appointment SelectedAppointment { get; set; }
        public List<Appointment> SelectedAppointmentTable { get; set; }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged(nameof(Duration));
            }
        }

        private Window _view;
        public Window View { get { return _view; } }

        public DateTime SelectedDateTime { get; set; }
        public ICommand BackCommand { get; }
        public ICommand UpdateAppointmentCommand { get; }
        private DoctorAppointmentsViewModel _viewModel;

        public UpdateAppointmentViewModel(Window view, DoctorAppointmentsViewModel viewModel)
        {

            _view = view;
            _viewModel = viewModel;

            SelectedAppointment = viewModel.SelectedAppointment;
            SelectedAppointmentTable = new List<Appointment> { SelectedAppointment };

            SelectedDateTime = viewModel.SelectedAppointment.TimeSlot.StartTime;

            Duration = (viewModel.SelectedAppointment.TimeSlot.EndTime - viewModel.SelectedAppointment.TimeSlot.StartTime).Minutes;
            
            UpdateAppointmentCommand = new UpdateAppointmentCommand(this);
            ((UpdateAppointmentCommand)UpdateAppointmentCommand).AppointmentUpdated += OnAppointmentUpdated;


        }

        private void OnAppointmentUpdated(object? sender, bool success)
        {
            if (success)
            {
                _viewModel.Appointments = _viewModel.LoadAppointments();
                MessageBox.Show("Succesfully updated appointment.");
            }
            else
            {
                MessageBox.Show("Failed to update appointment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
