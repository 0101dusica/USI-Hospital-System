

using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.UserManagement;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class InsertMedicalRecordCommand : BaseCommand
    {
        private InsertPatientViewModel _insertPatientViewModel;
        private InsertPatientView _insertPatientView;
        private NursePatientsListViewModel _viewModel;

        public InsertMedicalRecordCommand(InsertPatientViewModel insertPatientViewModel, InsertPatientView insertPatientView, NursePatientsListViewModel nursePatientsListViewModel)
        {
            _insertPatientViewModel = insertPatientViewModel;
            _insertPatientView = insertPatientView;
            _viewModel = nursePatientsListViewModel;
        }

        public override bool CanExecute(object? parameter)
        {
            string username = _insertPatientViewModel.Username;
            return _insertPatientViewModel.isUsernameUnique(username);
        }

        public override void Execute(object? parameter)
        {
            string firstName = _insertPatientViewModel.FirstName;
            string lastName = _insertPatientViewModel.LastName;
            string username = _insertPatientViewModel.Username;
            string password = _insertPatientViewModel.Password;


            if (_insertPatientViewModel.isInputForPatientEmpty(firstName, lastName, username, password))
            {
                User newPerson = new User(firstName, lastName, username, password, UserStatus.Active);
                MedicalRecord medicalRecord = new MedicalRecord();
                Patient newPatient = new Patient(newPerson, medicalRecord);

                _insertPatientView.Close();
                InsertMedicalRecordView insertMedicalRecordView = new InsertMedicalRecordView();
                insertMedicalRecordView.DataContext = new InsertMedicalRecordViewModel(insertMedicalRecordView, newPatient, _viewModel);
                insertMedicalRecordView.ShowDialog();
            }
        }


    }
}
