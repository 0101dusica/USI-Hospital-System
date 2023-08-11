using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel
{
    public class UpdateMedicalRecordViewModel : BaseViewModel
    {
        private PatientService _patientService;
        public Patient Patient { get; set; }
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        private int height;
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private int weight;
        public int Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        private ObservableCollection<string> allergies;
        public ObservableCollection<string> Allergies
        {
            get { return allergies; }
            set
            {
                allergies = value;
                OnPropertyChanged(nameof(Allergies));
            }
        }

        private string selectedAllergie;
        public string SelectedAllergie
        {
            get { return selectedAllergie; }
            set
            {
                selectedAllergie = value;
                OnPropertyChanged(nameof(SelectedAllergie));
            }
        }

        private string newAllergie;
        public string NewAllergie
        {
            get { return newAllergie; }
            set
            {
                newAllergie = value;
                OnPropertyChanged(nameof(NewAllergie));
            }
        }

        private ObservableCollection<string> medicalHistory;
        public ObservableCollection<string> MedicalHistory
        {
            get { return medicalHistory; }
            set
            {
                medicalHistory = value;
                OnPropertyChanged(nameof(MedicalHistory));
            }
        }

        private string selectedMedicalHistory;
        public string SelectedMedicalHistory
        {
            get { return selectedMedicalHistory; }
            set
            {
                selectedMedicalHistory = value;
                OnPropertyChanged(nameof(SelectedMedicalHistory));
            }
        }

        private string newMedicalHistory;
        public string NewMedicalHistory
        {
            get { return newMedicalHistory; }
            set
            {
                newMedicalHistory = value;
                OnPropertyChanged(nameof(NewMedicalHistory));
            }
        }

        private DoctorPatientListViewModel _viewModel;


        public ICommand UpdateMedicalRecordCommand { get; set; }
        public ICommand AddNewMedicalHistoryCommand { get; }
        public ICommand DeleteMedicalHistoryCommand { get; set; }
        public ICommand AddNewAllergieCommand { get; set; }
        public ICommand DeleteAllergieCommand { get; set; }



        public UpdateMedicalRecordViewModel(string patientUsername, DoctorPatientListViewModel viewModel = null)
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            Patient = _patientService.GetByUsername(patientUsername);

            _viewModel = viewModel;

            FirstName = Patient.FirstName;
            LastName = Patient.LastName;
            Height = Patient.MedicalRecord.Height;
            Weight = Patient.MedicalRecord.Weight;
            MedicalHistory = new ObservableCollection<string>(Patient.MedicalRecord.MedicalHistory);
            Allergies = new ObservableCollection<string>(Patient.MedicalRecord.Allergies);

            AddNewMedicalHistoryCommand = new AddNewMedicalHistoryCommand(this);
            DeleteMedicalHistoryCommand = new DeleteMedicalHistoryCommand(this);
            AddNewAllergieCommand = new AddNewAllergieCommand(this);
            DeleteAllergieCommand = new DeleteAllergieCommand(this);

            UpdateMedicalRecordCommand = new UpdateMedicalRecordCommand(this);
            ((UpdateMedicalRecordCommand)UpdateMedicalRecordCommand).MedicalRecordUpdated += OnMedicalRecordUpdated;
        }
        private void OnMedicalRecordUpdated(object sender, bool success)
        {
            if (success)
            {
                if (_viewModel != null)
                {
                    _viewModel.PatientsTable = _viewModel.LoadPatients();
                }
                MessageBox.Show("Medical record updated successfully.");
            }
            else
            {
                MessageBox.Show("Failed to update medical record.");
            }
        }
    }
}
