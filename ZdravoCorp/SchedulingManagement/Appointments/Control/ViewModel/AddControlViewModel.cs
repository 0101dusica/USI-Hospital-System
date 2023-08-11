using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Control.Command;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.Command;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.Control.ViewModel
{
    public class AddControlViewModel : BaseViewModel
    {
        private List<Patient> _patientTable;
        public List<Patient> PatientTable
        {
            get { return _patientTable; }
            set
            {
                _patientTable = value;
                OnPropertyChanged(nameof(PatientTable));
            }
        }

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

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged(nameof(Date));
            }
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        public ICommand AddControlCommand { get; }

        private Doctor _loggedDoctor;
        public Doctor LoggedDoctor
        {
            get { return _loggedDoctor; }
        }

        public HospitalCare SelectedHospitalCare { get; }

        private Window _view;
        public Window View
        {
            get { return _view; }
            set
            {
                _view = value;
                OnPropertyChanged(nameof(View));
            }
        }


        public AddControlViewModel(Window view,Doctor loggedDoctor, HospitalCare selectedHospitalCare)
        {
            _loggedDoctor = loggedDoctor;
            _view = view;
            SelectedHospitalCare = selectedHospitalCare;
            PatientService patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            PatientTable = new List<Patient> { patientService.GetByUsername(selectedHospitalCare.PatientUsername) };
            Duration = 15;

            Date = selectedHospitalCare.TimeSlot.EndTime.AddDays(10);
            AddControlCommand = new AddControlCommand(this);
            ((AddControlCommand)AddControlCommand).ControlAdded += OnControlAdded;

        }

        private void OnControlAdded(object? sender, bool success)
        {
            if (success)
            {
                MessageBox.Show("Control added successfully.");
            }
            else
            {
                MessageBox.Show("Failed to add control.");
            }
        }
    }
}
