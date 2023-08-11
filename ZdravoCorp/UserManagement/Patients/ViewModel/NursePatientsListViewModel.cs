using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.UserManagement.Patients.Command;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Patients.ViewModel
{
    public class NursePatientsListViewModel : BaseViewModel
    {
        //Bindings
        private PatientService _patientService { get; }

        public ICommand OpenPatientAdmissionWindowCommand { get; }
        public ICommand InsertPatientCommand { get; }
        public ICommand EditPatientCommand { get; }
        public ICommand ShowMedicalRecordCommand { get; }
        public ICommand DeletePatientCommand { get; }
        public ICommand BackCommand { get; }

        private ObservableCollection<Patient> _patientsTable;
        public ObservableCollection<Patient> PatientsTable
        {
            get { return _patientsTable; }
            set
            {
                _patientsTable = value;
                OnPropertyChanged(nameof(PatientsTable));
            }
        }
        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged(nameof(SelectedPatient));
            }
        }

        //Constructor

        public NursePatientsListViewModel()
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            PatientsTable = LoadPatients();

            InsertPatientCommand = new InsertPatientCommand(this);
            EditPatientCommand = new EditPatientCommand(this);
            DeletePatientCommand = new DeletePatientCommand(this);
            OpenPatientAdmissionWindowCommand = new OpenPatientAdmissionWindowCommand(this);
            ShowMedicalRecordCommand = new ShowMedicalRecordCommand<NursePatientsListViewModel>(this, OnMedicalRecordResult);

        }

        //Load Table
        public ObservableCollection<Patient> LoadPatients()
        {
            return new ObservableCollection<Patient>(_patientService.GetAll());
        }

        //Validations

        public bool isPatientSelected()
        {
            if (SelectedPatient == null)
            {
                MessageBox.Show($"Cannot submit without selected patient!");
                return false;
            }
            return true;
        }

        private void OnMedicalRecordResult(bool success)
        {
            if (!success)
            {
                MessageBox.Show("Please select a patient assigned to you.");
            }
        }


    }
}
