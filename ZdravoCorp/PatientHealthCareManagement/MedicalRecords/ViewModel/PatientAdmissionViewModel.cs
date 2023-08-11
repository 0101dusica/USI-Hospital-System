using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Patients.Command;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel
{
    public class PatientAdmissionViewModel : BaseViewModel
    {
        //Bindings

        private Patient selectedPatient { get; set; }
        private PatientAdmissionView _patientAdmissionView { get; set; }
        private NursePatientsListViewModel _nursePatientsListViewModel;
        public Appointment patientsAppointment { get; set; }

        public ICommand BackCommand { get; }
        public ICommand CreatePatientAdmissionCommand { get; }


        private ObservableCollection<string> _allergies = new ObservableCollection<string>();
        public ObservableCollection<string> Allergies
        {
            get { return _allergies; }
            set { _allergies = value; OnPropertyChanged(nameof(Allergies)); }
        }
        private string _newAllergie;
        public string NewAllergie
        {
            get { return _newAllergie; }
            set { _newAllergie = value; OnPropertyChanged(nameof(NewAllergie)); }
        }

        private string _selectedAllergie;
        public string SelectedAllergie
        {
            get { return _selectedAllergie; }
            set
            {
                if (_selectedAllergie != value)
                {
                    _selectedAllergie = value;
                    OnPropertyChanged(nameof(SelectedAllergie));
                }
            }
        }

        private ObservableCollection<string> _medicalDiseases = new ObservableCollection<string>();
        public ObservableCollection<string> MedicalHistory
        {
            get { return _medicalDiseases; }
            set { _medicalDiseases = value; OnPropertyChanged(nameof(MedicalHistory)); }
        }
        private string _newMedicalDisease;
        public string NewMedicalDisease
        {
            get { return _newMedicalDisease; }
            set { _newMedicalDisease = value; OnPropertyChanged(nameof(NewMedicalDisease)); }
        }

        private string _selectedMedicalDisease;
        public string SelectedMedicalDisease
        {
            get { return _selectedMedicalDisease; }
            set
            {
                if (_selectedMedicalDisease != value)
                {
                    _selectedMedicalDisease = value;
                    OnPropertyChanged(nameof(SelectedMedicalDisease));
                }
            }
        }

        private ObservableCollection<string> _symptoms = new ObservableCollection<string>();
        public ObservableCollection<string> Symptoms
        {
            get { return _symptoms; }
            set { _symptoms = value; OnPropertyChanged(nameof(Symptoms)); }
        }
        private string _newSymptom;
        public string NewSymptom
        {
            get { return _newSymptom; }
            set { _newSymptom = value; OnPropertyChanged(nameof(NewSymptom)); }
        }

        private string _selectedSymptom;
        public string SelectedSymptom
        {
            get { return _selectedSymptom; }
            set
            {
                if (_selectedSymptom != value)
                {
                    _selectedSymptom = value;
                    OnPropertyChanged(nameof(SelectedSymptom));
                }
            }
        }

        public ICommand AddNewAllergieCommand { get; }
        public ICommand DeleteAllergieCommand { get; }

        public ICommand AddNewMedicalDiseaseCommand { get; }
        public ICommand DeleteMedicalDiseaseCommand { get; }

        public ICommand AddNewSymptomCommand { get; }
        public ICommand DeleteSymptomCommand { get; }

        public ICommand AddPatientAdmissionCommand { get; }

        //Constructor

        public PatientAdmissionViewModel(Patient selectedPatient, Appointment earliestAppointment, PatientAdmissionView patientAdmissionView, NursePatientsListViewModel nursePatientsListViewModel)
        {
            this.selectedPatient = selectedPatient;
            patientsAppointment = earliestAppointment;
            _patientAdmissionView = patientAdmissionView;
            _nursePatientsListViewModel = nursePatientsListViewModel;

            setBindings();

            Allergies = new ObservableCollection<string>(selectedPatient.MedicalRecord.Allergies);
            MedicalHistory = new ObservableCollection<string>(selectedPatient.MedicalRecord.MedicalHistory);

            AddNewAllergieCommand = new RelayCommand(AddAllergie);
            DeleteAllergieCommand = new RelayCommand(DeleteAllergie);

            AddNewMedicalDiseaseCommand = new RelayCommand(AddMedicalDisease);
            DeleteMedicalDiseaseCommand = new RelayCommand(DeleteMedicalDisease);

            AddNewSymptomCommand = new RelayCommand(AddSymptom);
            DeleteSymptomCommand = new RelayCommand(DeleteSymptom);

            AddPatientAdmissionCommand = new AddPatientAdmissionCommand(_nursePatientsListViewModel, this, _patientAdmissionView, selectedPatient, patientsAppointment);



        }

        //functions that update fields using bindings

        public void setBindings()
        {
            _patientAdmissionView.UsernameTextBox.SetBinding(TextBox.TextProperty, new Binding("Username") { Source = selectedPatient });
            _patientAdmissionView.AppointmentsIDTextBox.SetBinding(TextBox.TextProperty, new Binding("Id") { Source = patientsAppointment });
        }

        public void AddAllergie()
        {

            if (!string.IsNullOrEmpty(NewAllergie))
            {
                Allergies.Add(NewAllergie);
                NewAllergie = string.Empty;
            }
        }

        public void DeleteAllergie()
        {
            if (!string.IsNullOrEmpty(SelectedAllergie))
            {
                Allergies.Remove(SelectedAllergie);
                SelectedAllergie = null;
            }

        }

        public void AddMedicalDisease()
        {

            if (!string.IsNullOrEmpty(NewMedicalDisease))
            {
                MedicalHistory.Add(NewMedicalDisease);
                NewMedicalDisease = string.Empty;
            }
        }

        public void DeleteMedicalDisease()
        {
            if (!string.IsNullOrEmpty(SelectedMedicalDisease))
            {
                MedicalHistory.Remove(SelectedMedicalDisease);
                SelectedMedicalDisease = null;
            }

        }

        public void AddSymptom()
        {
            if (!string.IsNullOrEmpty(NewSymptom))
            {
                Symptoms.Add(NewSymptom);
                NewSymptom = string.Empty;
            }

        }

        public void DeleteSymptom()
        {
            if (!string.IsNullOrEmpty(SelectedSymptom))
            {
                Symptoms.Remove(SelectedSymptom);
                SelectedSymptom = null;
            }
        }
    }
}
