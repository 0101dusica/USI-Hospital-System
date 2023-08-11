using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Doctors.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.PatientAppointment.ViewModel

{
    public class PatientAppointmentsViewModel : BaseViewModel
    {
        public DoctorService DoctorService { get; set; }
        public AppointmentService AppointmentService { get; set; }
        public PatientService PatientService { get; set; }
        public PatientAppointmentActionsService PatientAppointmentActionsService { get; set; }
        public Patient Patient { get; set; }
        public ICommand CreateAppointmentCommand { get; set; }
        public ICommand UpdateAppointmentCommand { get; set; }
        public ICommand DeleteAppointmentCommand { get; set; }

        private string _dateInputTextBox = string.Empty;
        public string DateInputText
        {
            get => _dateInputTextBox;
            set
            {
                _dateInputTextBox = value;
                OnPropertyChanged(nameof(DateInputText));
            }
        }

        private string _timeInputTextBox = string.Empty;
        public string TimeInputText
        {
            get => _timeInputTextBox;
            set
            {
                _timeInputTextBox = value;
                OnPropertyChanged(nameof(TimeInputText));
            }
        }

        private ObservableCollection<Doctor> _doctors = new ObservableCollection<Doctor>();
        public ObservableCollection<Doctor> Doctors
        {
            get => _doctors;
            set
            {
                _doctors = value;
                OnPropertyChanged(nameof(Doctors));
            }
        }

        private Doctor? _selectedDoctor;
        public Doctor? SelectedDoctor
        {
            get => _selectedDoctor;
            set
            {
                _selectedDoctor = value;
                OnPropertyChanged(nameof(SelectedDoctor));

            }
        }

        private ObservableCollection<Appointment>? _appointments;
        public ObservableCollection<Appointment>? Appointments
        {
            get => _appointments;
            set
            {
                _appointments = value;
                OnPropertyChanged(nameof(Appointments));
            }
        }

        private Appointment? _selectedAppointment;
        public Appointment? SelectedAppointment
        {
            get => _selectedAppointment;
            set
            {
                _selectedAppointment = value;
                OnPropertyChanged(nameof(SelectedAppointment));
                FillFields(SelectedAppointment);
            }
        }
        public PatientAppointmentsViewModel(PatientSearchDoctorsViewModel? viewModel, Patient loggedPatient, DoctorService doctorService,
            AppointmentService appointmentService, PatientService patientService,
            PatientAppointmentActionsService patientAppointmentActionsService)
        {

            Patient = loggedPatient;
            PatientService = patientService;
            AppointmentService = appointmentService;
            DoctorService = doctorService;
            PatientAppointmentActionsService = patientAppointmentActionsService;
            Doctors = new ObservableCollection<Doctor>(doctorService.GetAll());
            LoadDataGrid(AppointmentService);
            SelectedDoctor = null;
            if (viewModel != null)
            {
                if (viewModel.SelectedDoctor != null) SelectedDoctor = DoctorService.GetByUsername(viewModel.SelectedDoctor.Username);
            }
            CreateAppointmentCommand = new CreateAppointmentCommand(this, AppointmentService, DoctorService, PatientService);
            UpdateAppointmentCommand = new UpdateAppointmentCommand(this, AppointmentService, DoctorService, PatientService);
            DeleteAppointmentCommand = new DeleteAppointmentCommand(this, AppointmentService, PatientAppointmentActionsService);
        }

        public void LoadDataGrid(AppointmentService appointmentService)
        {
            Appointments = new ObservableCollection<Appointment>(appointmentService.GetScheduledAppointmentsForPatient(Patient));
        }

        public static bool IsTimeValid(string time)
        {
            DateTime result;
            return DateTime.TryParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out result);
        }
        public static bool IsDateValid(string date)
        {
            DateTime result;
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None,
                out result) && result >= DateTime.Now;
        }

        public bool ValidateInputs()
        {
            if (!IsDateValid(DateInputText) || !IsTimeValid(TimeInputText))
            {
                MessageBox.Show("Invalid date or time input!");
                return false;

            }

            if (SelectedDoctor != null) return true;
            MessageBox.Show("Doctor must be chosen!");
            return false;

        }

        public DateTime GetAppointmentDateTime(string date, string time)
        {
            DateTime dateAppointment = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date;
            DateTime timeAppointment = DateTime.ParseExact(time, "HH:mm:ss", CultureInfo.InvariantCulture);
            return dateAppointment.Date + timeAppointment.TimeOfDay;
        }

        public void FillFields(Appointment? selectedAppointment)
        {
            if (selectedAppointment == null) return;
            DateInputText = selectedAppointment!.TimeSlot.StartTime.Date.ToString("yyyy-MM-dd");
            TimeSpan time = selectedAppointment.TimeSlot.StartTime.TimeOfDay;
            string timeStr = $"{time}";
            TimeInputText = timeStr;
            SelectedDoctor = DoctorService.GetByUsername(selectedAppointment.DoctorUsername);
        }
        public bool CheckIsDate24HDiff(Appointment appointment)
        {
            DateTime now = DateTime.Now;
            DateTime endOf24Hrs = now.AddDays(1);
            return appointment.TimeSlot.StartTime >= now && appointment.TimeSlot.StartTime < endOf24Hrs;
        }
    }
}
