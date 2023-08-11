using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.View;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel
{
    public class DoctorAppointmentsViewModel : BaseViewModel
    {
        private ObservableCollection<Appointment> _appointments;
        public ObservableCollection<Appointment> Appointments
        {
            get { return _appointments; }
            set
            {
                _appointments = value;
                OnPropertyChanged(nameof(Appointments));
            }
        }

        private Appointment _selectedAppointment;
        public Appointment SelectedAppointment
        {
            get { return _selectedAppointment; }
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));
            }
        }
        private DateTime _selectedDate { get; set; }
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
                PerformUpdate(_selectedDate);

            }
        }

        private Doctor _loggedDoctor;
        public Doctor LoggedDoctor
        {
            get { return _loggedDoctor; }

        }
        private Window _view;
        public Window View { get { return _view; } }

        public ICommand ShowMedicalRecordCommand { get; }
        public ICommand StartExaminationCommand { get; }
        public ICommand CancelAppointmentCommand { get; }
        public ICommand ShowAddAppointmentCommand { get; }
        public ICommand ShowUpdateAppointmentCommand { get; }

        public DoctorAppointmentsViewModel(Doctor loggedDoctor)
        {
            _loggedDoctor = loggedDoctor;
            SelectedDate = DateTime.Now;

            Appointments = LoadAppointments();
            ShowAddAppointmentCommand = new RelayCommand(ShowAddAppointment);
            StartExaminationCommand = new StartExaminationCommand(this);
            ((StartExaminationCommand)StartExaminationCommand).AppointmentStarted += OnAppointmentStarted;

            ShowMedicalRecordCommand = new ShowMedicalRecordCommand<DoctorAppointmentsViewModel>(this, OnResult);
            CancelAppointmentCommand = new CancelAppointmentCommand(this);
            ((CancelAppointmentCommand)CancelAppointmentCommand).AppointmentCancelled += OnAppointmentCancelled;
            ShowUpdateAppointmentCommand = new ShowUpdateAppointmentCommand(this, OnResult);
        }

        public DoctorAppointmentsViewModel(Window view, Doctor loggedDoctor)
        {
            _loggedDoctor = loggedDoctor;
            SelectedDate = DateTime.Now;
            _view = view;

            Appointments = LoadAppointments();
            ShowAddAppointmentCommand = new RelayCommand(ShowAddAppointment);
            StartExaminationCommand = new StartExaminationCommand(this);
            ((StartExaminationCommand)StartExaminationCommand).AppointmentStarted += OnAppointmentStarted;

            ShowMedicalRecordCommand = new ShowMedicalRecordCommand<DoctorAppointmentsViewModel>(this, OnResult);
            CancelAppointmentCommand = new CancelAppointmentCommand(this);
            ((CancelAppointmentCommand)CancelAppointmentCommand).AppointmentCancelled += OnAppointmentCancelled;
            ShowUpdateAppointmentCommand = new ShowUpdateAppointmentCommand(this, OnResult);
        }
        private void OnResult(bool success)
        {
            if (!success)
            {
                MessageBox.Show("Please select one row with a scheduled appointment.");
            }
        }
        private void ShowAddAppointment()
        {
            var view = new CreateAppointmentView();
            view.DataContext = new CreateAppointmentViewModel(_loggedDoctor, this);
            view.ShowDialog();
        }
        private void OnAppointmentStarted(object sender, bool success)
        {
            if (!success)
            {
                MessageBox.Show($"Selected appointment is not currently available for examination.");
            }
        }

        private void OnAppointmentCancelled(object sender, bool success)
        {
            if (success)
            {
                SelectedAppointment.AppointmentStatus = AppointmentStatus.Canceled;
                CollectionViewSource.GetDefaultView(Appointments).Refresh();
                MessageBox.Show("Appointment cancelled successfully.");
            }
            else
            {
                MessageBox.Show("Failed to cancel appointment.");
            }
        }
        private void PerformUpdate(DateTime selectedDate)
        {
            AppointmentService appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>())); //promenicess
            Appointments = new ObservableCollection<Appointment>(appointmentService.GetAppointmentsWithinThreeDays(selectedDate, _loggedDoctor));
        }

        public ObservableCollection<Appointment> LoadAppointments()
        {
            AppointmentService appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            return new ObservableCollection<Appointment>(appointmentService.GetDoctorAppointments(_loggedDoctor)); //napravi funckiju samo za ulgovanog doktora
        }
    }
}
