using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.CommunicatonManagement.Notifications.Command;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.CommunicatonManagement.Notifications.ViewModel
{
    public class PatientNotificationViewModel : BaseViewModel
    {
        public PrescriptionService PrescriptionService { get; set; }
        public NotificationService NotificationService { get; set; }
        public PatientNotificationService PatientNotificationService { get; set; }
        public MedicineService MedicineService { get; set; }

        private Patient? _loggedPatient;

        public ICommand CreatePatientNotificationCommand { get; set; }

        public ICommand ChangePrescriptionNotificationCommand { get; set; }

        private ObservableCollection<Prescription>? _prescriptionNotifications;

        public ObservableCollection<Prescription>? PrescriptionNotifications

        {
            get => _prescriptionNotifications;
            set
            {
                _prescriptionNotifications = value;
                OnPropertyChanged(nameof(PrescriptionNotifications));
            }
        }

        private ObservableCollection<Notification>? _otherNotifications;

        public ObservableCollection<Notification>? OtherNotifications
        {
            get => _otherNotifications;
            set
            {
                _otherNotifications = value;
                OnPropertyChanged(nameof(OtherNotifications));
            }
        }
        private string? _title;
        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string? _startTime;
        public string? StartDate
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartDate));

            }
        }
        private string? _endTime;
        public string? EndDate
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged(nameof(EndDate));

            }
        }
        private string? _timeSpan;
        public string? Time
        {
            get => _timeSpan;
            set
            {
                _timeSpan = value;
                OnPropertyChanged(nameof(Time));

            }
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string? _timeSet;
        public string? TimeSet
        {
            get => _timeSet;
            set
            {
                _timeSet = value;
                OnPropertyChanged(nameof(TimeSet));

            }
        }

        private Prescription? _selectedPrescription;
        public Prescription? SelectedPrescription
        {
            get => _selectedPrescription;
            set
            {
                _selectedPrescription = value;
                OnPropertyChanged(nameof(SelectedPrescription));
                FillFields(SelectedPrescription);
            }
        }

        private void FillFields(Prescription? selectedPrescription)
        {
            if (selectedPrescription != null) TimeSet = selectedPrescription!.TimeSet.ToString();
        }

        public PatientNotificationViewModel(NotificationService notificationService,
            PrescriptionService prescriptionService, MedicineService medicineService,
            PatientNotificationService patientNotificationService, Patient patient)
        {
            _loggedPatient = patient;
            NotificationService = notificationService;
            PrescriptionService = prescriptionService;
            MedicineService = medicineService;
            PatientNotificationService = patientNotificationService;
            PrescriptionNotifications = new ObservableCollection<Prescription>(PrescriptionService.GetAll());
            OtherNotifications = new ObservableCollection<Notification>(NotificationService.GetAll());
            CreatePatientNotificationCommand =
                new CreatePatientNotificationCommand(this, _loggedPatient, NotificationService);
            ChangePrescriptionNotificationCommand = new ChangePrescriptionNotificationCommand(this, _loggedPatient, PrescriptionService);

        }
        public void LoadDataGrid(PrescriptionService prescriptionService)
        {
            PrescriptionNotifications = new ObservableCollection<Prescription>(prescriptionService.GetAll());
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

            if (!IsDateValid(StartDate!) || !IsTimeValid(Time!) || !IsDateValid(EndDate!))
            {
                MessageBox.Show("Invalid date or time input!");
                return false;
            }
            else if (Description == null || Description!.Length < 10 || Title == null || Title!.Length < 5)
            {
                MessageBox.Show("Invalid description or title input!");
                return false;
            }
            else if (!CheckDates(StartDate, EndDate))
            {
                MessageBox.Show("Invalid date input!");
                return false;
            }

            return true;
        }

        private bool CheckDates(string? startDate, string? endDate)
        {
            DateTime startDateTime, endDateTime;
            if (!DateTime.TryParse(startDate, out startDateTime) || !DateTime.TryParse(endDate, out endDateTime)) return false;
            return endDateTime >= startDateTime;
        }

        public bool ValidateTimeSet()
        {
            int number;
            if (int.TryParse(TimeSet, out int result))
            {
                number = result;
                if (number < 1 || number > 600)
                {
                    MessageBox.Show("Must be number between 1 and 600!");
                    return false;
                }
                return true;
            }
            MessageBox.Show("Must be number!");
            return false;

        }
    }
}