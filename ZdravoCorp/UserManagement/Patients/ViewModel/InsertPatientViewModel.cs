using System.Windows;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Patients.ViewModel
{
    public class InsertPatientViewModel : BaseViewModel
    {
        //Bindings
        private NursePatientsListViewModel _viewModel;
        private InsertPatientView _insertPatientView { get; set; }
        private PatientService _patientService;
        public ICommand InsertMedicalRecordCommand { get; }
        public ICommand BackCommand { get; }
        private Patient newPatient { get; set; }



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

        public string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        //Constructor

        public InsertPatientViewModel(InsertPatientView insertPatientView, NursePatientsListViewModel nursePatientsListViewModel)
        {
            _insertPatientView = insertPatientView;
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _viewModel = nursePatientsListViewModel;
            InsertMedicalRecordCommand = new InsertMedicalRecordCommand(this, _insertPatientView, _viewModel);

        }

        //Validations

        public bool isUsernameUnique(string username)
        {
            foreach (Patient patient in _patientService.GetAll())
            {
                if (username == patient.Username)
                {
                    MessageBox.Show($"Username already exist!");
                    return false;
                }
            }

            return true;
        }


        public bool isInputForPatientEmpty(string firstName, string lastName, string username, string password)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show($"Cannot submit an empty form!");
                return false;
            }
            else if (!char.IsUpper(firstName[0]) || !char.IsUpper(lastName[0]))
            {
                MessageBox.Show($"Check again! First and last name must start with a capital letter!");
                return false;
            }

            return true;
        }

    }
}
