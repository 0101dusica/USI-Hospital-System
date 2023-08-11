using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.View;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.ViewModel;
using ZdravoCorp.SchedulingManagement.FreeDays.View;
using ZdravoCorp.SchedulingManagement.FreeDays.ViewModel;
using ZdravoCorp.UserManagement.Authentication.Login.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.View;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.ViewModel
{
    public class MenuDoctorViewModel : BaseViewModel
    {
        private MenuDoctorView _menuDoctorView { get; }
        private DoctorService _doctorService;
        private Doctor _loggedDoctor;

        public ICommand LogoutCommand { get; }
        public ICommand ShowAppointmentTableCommand { get; }
        public ICommand ShowFreeDaysCommand { get; }
        public ICommand ShowPatientTableCommand { get; }
        public ICommand ShowHospitalCareVisitsCommand { get; }

        public ICommand ShowChooseRecipientCommand { get; }
        public MenuDoctorViewModel(MenuDoctorView menuDoctorView, DoctorService doctorService, Doctor loggedDoctor)
        {
            _menuDoctorView = menuDoctorView;
            _doctorService = doctorService;
            _loggedDoctor = loggedDoctor;
            LogoutCommand = new LogoutCommand(_menuDoctorView);

            ShowPatientTableCommand = new ShowPatientTableCommand(_loggedDoctor);
            ShowAppointmentTableCommand = new ShowAppointmentTableCommand(_loggedDoctor);
            ShowHospitalCareVisitsCommand = new RelayCommand(ShowHospitalCareVisits);
            ShowFreeDaysCommand = new RelayCommand(ShowFreeDays);
            ShowChooseRecipientCommand = new ShowChooseRecipientCommand(loggedDoctor);
        }

        private void ShowHospitalCareVisits()
        {
            var view = new HospitalCareTableView();
            view.DataContext = new HospitalCareTableViewModel(_loggedDoctor);
            view.ShowDialog();
        }
        private void ShowFreeDays()
        {
            var view = new AddFreeDaysView();
            view.DataContext = new AddFreeDaysViewModel(_loggedDoctor);
            view.ShowDialog();
        }

    }
}
