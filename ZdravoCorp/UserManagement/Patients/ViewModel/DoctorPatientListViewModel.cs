using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Patients.ViewModel
{
    public class DoctorPatientListViewModel : BaseViewModel
    {

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

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                PerformUpdate(_searchText);
            }
        }

        public ICommand ShowMedicalRecordCommand { get; }
        public ICommand ShowUpdateMedicalRecordCommand { get; }

        private Doctor _loggedDoctor;
        public Doctor LoggedDoctor
        {
            get { return _loggedDoctor; }
            set { _loggedDoctor = value; }
        }


        public DoctorPatientListViewModel(Doctor loggedDoctor)
        {
            PatientsTable = LoadPatients(); ;
            _loggedDoctor = loggedDoctor;

            ShowMedicalRecordCommand = new ShowMedicalRecordCommand<DoctorPatientListViewModel>(this, OnMedicalRecordResult);
            ShowUpdateMedicalRecordCommand = new ShowUpdateMedicalRecordCommand<DoctorPatientListViewModel>(this, OnMedicalRecordResult);

        }
        public ObservableCollection<Patient> LoadPatients()
        {
            PatientService service = new PatientService(new PatientRepository(new Serializer<Patient>()));
            return new ObservableCollection<Patient>(service.GetAll());
        }
        private void PerformUpdate(string searchText)
        {
            PatientService service = new PatientService(new PatientRepository(new Serializer<Patient>()));
            PatientsTable = new ObservableCollection<Patient>(service.GetMatchingPatients(searchText));
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
