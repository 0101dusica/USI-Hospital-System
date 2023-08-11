using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.Commands
{
    public class ShowPatientTableCommand : BaseCommand
    {
        private PatientService _patientService;
        private Doctor _loggedDoctor;

        public ShowPatientTableCommand(Doctor loggedDoctor)
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _loggedDoctor = loggedDoctor;
        }
        public override void Execute(object? parameter)
        {
            DoctorPatientListView doctorPatientListView = new DoctorPatientListView();
            doctorPatientListView.DataContext = new DoctorPatientListViewModel(_loggedDoctor);
            doctorPatientListView.ShowDialog();
        }
    }
}
