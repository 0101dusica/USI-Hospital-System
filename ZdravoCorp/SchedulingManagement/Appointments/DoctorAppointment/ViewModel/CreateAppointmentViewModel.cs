using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel
{
    public class CreateAppointmentViewModel : BaseViewModel
    {
        public Doctor LoggedDoctor { get; set; }
        public List<Patient> PatientsTable { get; set; }
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

        public IEnumerable<AppointmentType> AppointmentTypes => Enum.GetValues(typeof(AppointmentType)).Cast<AppointmentType>();
        private AppointmentType _selectedAppointmentType;
        public AppointmentType SelectedAppointmentType
        {
            get { return _selectedAppointmentType; }
            set
            {
                _selectedAppointmentType = value;
                OnPropertyChanged(nameof(SelectedAppointmentType));

                if (_selectedAppointmentType == AppointmentType.Appointment || _selectedAppointmentType == AppointmentType.EmergencyAppointment)
                {
                    Duration = 15;
                    IsDurationReadOnly = true;
                }
                else
                {
                    IsDurationReadOnly = false;
                }
            }
        }

        private bool _isDurationReadOnly;
        public bool IsDurationReadOnly
        {
            get { return _isDurationReadOnly; }
            set
            {
                _isDurationReadOnly = value;
                OnPropertyChanged(nameof(IsDurationReadOnly));
            }
        }

        public Patient SelectedPatient { get; set; }

        public DateTime SelectedDateTime { get; set; }

        private DoctorAppointmentsViewModel _viewModel;
        public ICommand AddAppointmentCommand { get; }
        public CreateAppointmentViewModel(Doctor loggedDoctor, DoctorAppointmentsViewModel viewModel)
        {
            LoggedDoctor = loggedDoctor;
            _viewModel = viewModel;
            PatientService patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            PatientsTable = patientService.GetAll();
            Duration = 15;

            AddAppointmentCommand = new AddAppointmentCommand(this);
            ((AddAppointmentCommand)AddAppointmentCommand).AppointmentAdded += OnAppointmentAdded;

        }

        private void OnAppointmentAdded(object sender, bool success)
        {
            if (success)
            {
                _viewModel.Appointments = _viewModel.LoadAppointments();
                MessageBox.Show("Appointment added successfully.");
            }
            else
            {
                MessageBox.Show("Failed to add appointment.");
            }
        }
    }
}
