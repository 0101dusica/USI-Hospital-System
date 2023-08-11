using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Patients.Command
{
    public class SaveEditedPatientCommand : BaseCommand
    {
        private Patient _selectedPatient;
        private PatientService _patientsService;
        private NursePatientsListViewModel _nursePatientsListViewModel;
        private EditPatientViewModel _editPatientViewModel;
        private EditPatientView _editPatientView;

        public SaveEditedPatientCommand(NursePatientsListViewModel nursePatientsListViewModel, Patient selectedPatient, EditPatientViewModel editPatientViewModel, EditPatientView editPatientView)
        {
            _patientsService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _selectedPatient = selectedPatient;
            _nursePatientsListViewModel = nursePatientsListViewModel;
            _editPatientViewModel = editPatientViewModel;
            _editPatientView = editPatientView;
        }

        public override void Execute(object? parameter)
        {
            string firstName = _editPatientViewModel.FirstName;
            string lastName = _editPatientViewModel.LastName;
            string password = _editPatientViewModel.Password;
            UserStatus userStatus;
            if (_editPatientViewModel.IsActiveStatusSelected)
            {
                userStatus = UserStatus.Active;
            }
            else
            {
                userStatus = UserStatus.Blocked;
            }
            string height = _editPatientViewModel.Height;
            string weight = _editPatientViewModel.Weight;
            ObservableCollection<string> medicalHistory = _editPatientViewModel.MedicalHistory;


            if (_editPatientViewModel.isInputForEditEmpty(firstName, lastName, password, height, weight))
            {
                MedicalRecord editedMedicalRecord = new MedicalRecord();
                Console.WriteLine(_editPatientViewModel.FirstName);
                editedMedicalRecord.Height = int.Parse(height);
                editedMedicalRecord.Weight = int.Parse(weight);
                editedMedicalRecord.MedicalHistory = medicalHistory.ToList();

                //_patientsService.Update(_selectedPatient.Username, _editPatientViewModel.FirstName, _editPatientViewModel.LastName, _editPatientViewModel.Password, userStatus, editedMedicalRecord);
                _editPatientView.Close();

                _nursePatientsListViewModel.PatientsTable = _nursePatientsListViewModel.LoadPatients();
            }
        }
    }
}
