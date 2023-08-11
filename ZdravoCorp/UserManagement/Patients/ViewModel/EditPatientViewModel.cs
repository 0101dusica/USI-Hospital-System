
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.UserManagement.Patients.Command;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Patients.ViewModel
{
    public class EditPatientViewModel : BaseViewModel
    {
        //Bindings

        private NursePatientsListViewModel _viewModel { get; set; }
        private EditPatientView _editPatientView { get; set; }
        public Patient? Patient { get; set; }
        private PatientService _patientService;
        public ICommand SaveEditedPatientCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand AddNewMedicalDiseaseCommand { get; }
        public ICommand DeleteMedicalDiseaseCommand { get; }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _weight;
        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        private string _height;
        public string Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private ObservableCollection<string> _medicalHistory = new ObservableCollection<string>();
        public ObservableCollection<string> MedicalHistory
        {
            get { return _medicalHistory; }
            set { _medicalHistory = value; OnPropertyChanged(nameof(MedicalHistory)); }
        }

        private string _newMedicalHistory;
        public string NewMedicalHistory
        {
            get { return _newMedicalHistory; }
            set { _newMedicalHistory = value; OnPropertyChanged(nameof(NewMedicalHistory)); }
        }

        private string _selectedMedicalHistory;
        public string SelectedMedicalHistory
        {
            get { return _selectedMedicalHistory; }
            set
            {
                if (_selectedMedicalHistory != value)
                {
                    _selectedMedicalHistory = value;
                    OnPropertyChanged(nameof(SelectedMedicalHistory));
                }
            }
        }

        private bool _isActiveStatusSelected;

        public bool IsActiveStatusSelected
        {
            get { return _isActiveStatusSelected; }
            set
            {
                _isActiveStatusSelected = value;
            }
        }

        private bool _isBlockedStatusSelected;

        public bool IsBlockedStatusSelected
        {
            get { return _isBlockedStatusSelected; }
            set
            {
                _isBlockedStatusSelected = value;
            }
        }

        //Constructor

        public EditPatientViewModel(Patient selectedPatient, EditPatientView editPatientView, NursePatientsListViewModel nursePatientsListViewModel)
        {
            _viewModel = nursePatientsListViewModel;
            _editPatientView = editPatientView;
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            Patient = _patientService.GetByUsername(selectedPatient.Username);

            MedicalHistory = new ObservableCollection<string>(Patient.MedicalRecord.MedicalHistory);
            FirstName = Patient.FirstName;
            LastName = Patient.LastName;
            Username = Patient.Username;
            Password = Patient.Password;
            Weight = Patient.MedicalRecord.Weight.ToString();
            Height = Patient.MedicalRecord.Height.ToString();
            if (Patient.UserStatus == 0)
            {
                IsActiveStatusSelected = true;
            }
            else
            {
                IsBlockedStatusSelected = false;
            }

            AddNewMedicalDiseaseCommand = new RelayCommand(AddNewMedicalDisease);
            DeleteMedicalDiseaseCommand = new RelayCommand(DeleteMedicalDisease);

            SaveEditedPatientCommand = new SaveEditedPatientCommand(_viewModel, Patient, this, _editPatientView);

        }

        //Validations

        public bool isInputForEditEmpty(string firstName, string lastName, string password, string height, string weight)
        {
            int number;
            bool isHeightInteger = int.TryParse(height, out number);
            bool isWeightInteger = int.TryParse(weight, out number);
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(height) || string.IsNullOrEmpty(weight))
            {
                MessageBox.Show($"Check again! Some input fields are EMPTY!");
                return false;
            }
            else if (!char.IsUpper(firstName[0]) || !char.IsUpper(lastName[0]))
            {
                MessageBox.Show($"Check again! First and last name must start with a capital letter!");
                return false;
            }
            else if (!isHeightInteger || !isWeightInteger)
            {
                MessageBox.Show($"Check again! Height or weight is WRONG!");
                return false;
            }


            return true;
        }

        //functions that update fields in listBox

        public void AddNewMedicalDisease()
        {

            if (!string.IsNullOrEmpty(NewMedicalHistory))
            {
                MedicalHistory.Add(NewMedicalHistory);
                NewMedicalHistory = string.Empty;
            }
        }

        public void DeleteMedicalDisease()
        {
            if (!string.IsNullOrEmpty(SelectedMedicalHistory))
            {
                MedicalHistory.Remove(SelectedMedicalHistory);
                SelectedMedicalHistory = null;
            }
        }
    }
}
