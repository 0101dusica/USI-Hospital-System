using System;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.UserManagement.Administrators.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Nurses.Service;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Authentication.Login.Command;
using ZdravoCorp.UserManagement.Authentication.Login.View;
using ZdravoCorp.UserManagement.Nurses.Repository;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.UserManagement.Administrators.Repository;
using ZdravoCorp.UserManagement.Administrators.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;

namespace ZdravoCorp.UserManagement.Authentication.Login.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginView LoginView;
        public ICommand LoginCommand { get; }

        private AdministratorService _administratorService;
        private DoctorService _doctorService;
        private NurseService _nurseService;
        private PatientService _patientService;

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public LoginViewModel(LoginView loginView)
        {
            _administratorService = new AdministratorService(new AdministratorRepository(new Serializer<Administrator>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _nurseService = new NurseService(new NurseRepository(new Serializer<Nurse>()));
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            LoginView = loginView;
            LoginCommand = new LoginCommand(this, _administratorService, _doctorService, _nurseService, _patientService, OnLoginResult);

        }
        private void OnLoginResult(bool success)
        {
            if (!success)
            {
                MessageBox.Show("Login failed. Invalid username or password.");
            }
        }
    }
}
