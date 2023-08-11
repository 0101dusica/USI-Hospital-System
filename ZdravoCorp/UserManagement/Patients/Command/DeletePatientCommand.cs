using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Patients.Command
{
    public class DeletePatientCommand : BaseCommand
    {
        private PatientService _patientService;
        private NursePatientsListViewModel _nursePatientsListViewModel;

        public DeletePatientCommand(NursePatientsListViewModel nursePatientsListViewModel)
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _nursePatientsListViewModel = nursePatientsListViewModel;
        }

        public override void Execute(object? parameter)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _patientService.Delete(_nursePatientsListViewModel.SelectedPatient);
            }
            _nursePatientsListViewModel.PatientsTable = _nursePatientsListViewModel.LoadPatients();
        }
    }
}
