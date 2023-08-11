using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Command;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.View;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.ViewModel
{
    public class AddEmergencyAppointmentViewModel : BaseViewModel
    {
        //Bindgings
        public ICommand GetEmergencyAppointmentDataCommand { get; }
        public ICommand SaveEmergencyAppointmentCommand { get; }
        public ICommand InsertSurgeryDurationCommand { get; }
        public ICommand AppointmentCommand { get; }
        public int emergencyAppointmentDuration { get; set; }
        public Dictionary<string, string> potentialTakenTerms { get; set; }
        public AppointmentType appointmentType { get; set; }
        public bool isAllTermsTaken { get; set; }


        private PatientService _patientService;
        private DoctorService _doctorService;

        public List<Patient> PatientsTable { get; set; }
        public Patient SelectedPatient { get; set; }

        public string SelectedSpecialization { get; set; }
        private List<string> specializationTable;
        public List<string> SpecializationTable
        {
            get { return specializationTable; }
            set
            {
                specializationTable = value;
                OnPropertyChanged("SpecializationTable");
            }
        }

        private string _searchPatient;
        public string SearchPatient
        {
            get { return _searchPatient; }
            set
            {
                _searchPatient = value;
                OnPropertyChanged(nameof(SearchPatient));
                PerformUpdatePatients(_searchPatient);
            }
        }

        private string _searchSpecialization;
        public string SearchSpecialization
        {
            get { return _searchSpecialization; }
            set
            {
                _searchSpecialization = value;
                OnPropertyChanged(nameof(SearchSpecialization));
                PerformUpdateSpecializations(_searchSpecialization);
            }
        }


        private string _specializedDoctor;
        public string SpecializedDoctor
        {
            get { return _specializedDoctor; }
            set
            {
                _specializedDoctor = value;
                OnPropertyChanged(nameof(SpecializedDoctor));
            }
        }

        private string _startDate;
        public string StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private string _startTime;
        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged(nameof(StartTime));
            }
        }

        private ObservableCollection<string> _terms = new ObservableCollection<string>();
        public ObservableCollection<string> Terms
        {
            get { return _terms; }
            set { _terms = value; OnPropertyChanged(nameof(Terms)); }
        }

        private string _selectedTerm;
        public string SelectedTerm
        {
            get { return _selectedTerm; }
            set
            {
                if (_selectedTerm != value)
                {
                    _selectedTerm = value;
                    OnPropertyChanged(nameof(SelectedTerm));
                }
            }
        }

        private bool _isAppointmentSelected;
        public bool IsAppointmentSelected
        {
            get { return _isAppointmentSelected; }
            set
            {
                _isAppointmentSelected = value;
            }
        }

        private bool _isSurgerySelected;
        public bool IsSurgerySelected
        {
            get { return _isSurgerySelected; }
            set
            {
                _isSurgerySelected = value;
            }
        }

        private string _surgeryDuration;
        public string SurgeryDuration
        {
            get { return _surgeryDuration; }
            set
            {
                _surgeryDuration = value;
                OnPropertyChanged(nameof(SurgeryDuration));
            }
        }

        private Visibility _createEmergencyAppointmentVisibility;
        public Visibility CreateEmergencyAppointmentVisibility
        {
            get { return _createEmergencyAppointmentVisibility; }
            set
            {
                _createEmergencyAppointmentVisibility = value;
                OnPropertyChanged(nameof(CreateEmergencyAppointmentVisibility));
            }
        }

        private Visibility _getDataVisibility;
        public Visibility GetDataVisibility
        {
            get { return _getDataVisibility; }
            set
            {
                _getDataVisibility = value;
                OnPropertyChanged(nameof(GetDataVisibility));
            }
        }

        private Visibility _surgeryDurationVisibility;
        public Visibility SurgeryDurationVisibility
        {
            get { return _surgeryDurationVisibility; }
            set
            {
                _surgeryDurationVisibility = value;
                OnPropertyChanged(nameof(SurgeryDurationVisibility));
            }
        }


        //Constructor

        public AddEmergencyAppointmentViewModel(AddEmergencyAppointmentView addEmergencyAppointmentView)
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            emergencyAppointmentDuration = 0;
            potentialTakenTerms = new Dictionary<string, string>();
            isAllTermsTaken = false;
            SurgeryDurationVisibility = Visibility.Collapsed;
            PatientsTable = _patientService.GetAll();
            SpecializationTable = _doctorService.GetAllSpecializations();
            DateTime currentTime = DateTime.Now;
            DateTime twoHoursFromNow = currentTime.AddHours(2);


            GetEmergencyAppointmentDataCommand = new GetEmergencyAppointmentDataCommand(this, currentTime, twoHoursFromNow);
            SaveEmergencyAppointmentCommand = new SaveEmergencyAppointmentCommand(this);

            InsertSurgeryDurationCommand = new RelayCommand(SetSurgeryDurationTextBoxVisible);
            AppointmentCommand = new RelayCommand(SetSurgeryDurationTextBoxHiden);

        }

        //functions that update fields using bindings

        public void SetSurgeryDurationTextBoxVisible()
        {
            SurgeryDurationVisibility = Visibility.Visible;
        }

        public void SetSurgeryDurationTextBoxHiden()
        {
            SurgeryDurationVisibility = Visibility.Hidden;
        }




        private void PerformUpdatePatients(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                PatientsTable = _patientService.GetAll();
            }
            else
            {
                PatientsTable = PatientsTable.Where(patient =>
                patient.FirstName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                || patient.LastName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                || patient.Username.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
            }

            OnPropertyChanged(nameof(PatientsTable));


        }
        private void PerformUpdateSpecializations(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                SpecializationTable = _doctorService.GetAllSpecializations();
            }
            else
            {
                SpecializationTable = SpecializationTable.Where(specialization =>
                specialization.Contains(searchText, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
            }
            OnPropertyChanged(nameof(SpecializationTable));
        }



        public void CheckEmergencyDuration()
        {
            if (_isAppointmentSelected)
            {
                emergencyAppointmentDuration = 15;
                appointmentType = AppointmentType.EmergencyAppointment;

            }
            else if (_isSurgerySelected)
            {
                emergencyAppointmentDuration = int.Parse(_surgeryDuration) * 60;
                appointmentType = AppointmentType.EmergencyAppointment;

            }
        }

        //Validations
        public bool ValidateSurgeryDurationInput(string surgeryDurationInput)
        {
            int parsedValue;
            if (int.TryParse(surgeryDurationInput, out parsedValue))
            {
                return parsedValue >= 0 && parsedValue <= 2;
            }
            else if (string.IsNullOrEmpty(surgeryDurationInput))
            {
                return false;
            }
            return false;
        }

    }
}
