using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.UserManagement.Administrators.Model;
using ZdravoCorp.UserManagement.Administrators.Service;
using ZdravoCorp.UserManagement.Authentication.Login.ViewModel;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.View;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuAdministrator.ViewModel;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.View;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.ViewModel;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.View;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.ViewModel;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.View;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.ViewModel;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.UserManagement.Nurses.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Login.Command
{
    public class LoginCommand : BaseCommand
    {
        private LoginViewModel _loginViewModel;

        private AdministratorService _administratorService;
        private DoctorService _doctorService;
        private NurseService _nurseService;
        private PatientService _patientService;

        private readonly Action<bool> _loginResultCallback;

        public LoginCommand(LoginViewModel loginViewModel, AdministratorService administratorService, DoctorService doctorService, NurseService nurseService, PatientService patientService, Action<bool> loginResultCallback)
        {
            _loginViewModel = loginViewModel;
            _administratorService = administratorService;
            _doctorService = doctorService;
            _nurseService = nurseService;
            _patientService = patientService;
            _loginResultCallback = loginResultCallback;
        }

        public override void Execute(object? parameter)
        {
            string username = _loginViewModel.Username;
            string password = _loginViewModel.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                _loginResultCallback?.Invoke(false);
                return;
            }

            var administrator = _administratorService.GetByUsername(username);
            var doctor = _doctorService.GetByUsername(username);
            var nurse = _nurseService.GetByUsername(username);
            var patient = _patientService.GetByUsername(username);


            switch (true)
            {
                case var _ when administrator != null && administrator.Password == password:
                    ShowAdministratorView(administrator);
                    _loginResultCallback?.Invoke(true);
                    break;
                case var _ when doctor != null && doctor.Password == password:
                    ShowDoctorView(doctor);
                    _loginResultCallback?.Invoke(true);
                    break;
                case var _ when nurse != null && nurse.Password == password:
                    ShowNurseView(nurse);
                    _loginResultCallback?.Invoke(true);
                    break;
                case var _ when patient != null && patient.Password == password:
                    ShowPatientView(patient);
                    _loginResultCallback?.Invoke(true);
                    break;
                default:
                    _loginResultCallback?.Invoke(false);
                    return;
            }
        }

        private void ShowAdministratorView(Administrator administrator)
        {
            MenuAdministratorView menuAdministratorView = new MenuAdministratorView();
            menuAdministratorView.DataContext = new MenuAdministratorViewModel(_administratorService, menuAdministratorView);
            _loginViewModel.LoginView.Close();
            menuAdministratorView.ShowDialog();
        }

        private void ShowDoctorView(Doctor doctor)
        {
            Doctor loggedDoctor = doctor;
            MenuDoctorView menuDoctorView = new MenuDoctorView();
            menuDoctorView.DataContext = new MenuDoctorViewModel(menuDoctorView, _doctorService, loggedDoctor);
            _loginViewModel.LoginView.Close();
            menuDoctorView.ShowDialog();
        }

        private void ShowNurseView(Nurse nurse)
        {
            Nurse loggedNurse = nurse;
            MenuNurseView menuNurseView = new MenuNurseView();
            menuNurseView.DataContext = new MenuNurseViewModel(menuNurseView, loggedNurse);
            _loginViewModel.LoginView.Close();
            menuNurseView.ShowDialog();
        }

        private void ShowPatientView(Patient patient)
        {
            Patient loggedPatient = patient;
            MenuPatientView menuPatientView = new MenuPatientView();
            menuPatientView.DataContext = new MenuPatientViewModel(menuPatientView, _patientService, loggedPatient);
            _loginViewModel.LoginView.Close();
            menuPatientView.ShowDialog();
        }
    }
}
