using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class UpdateMedicalRecordCommand : BaseCommand
    {
        private PatientService _patientService;

        private UpdateMedicalRecordViewModel _viewModel;

        public event EventHandler<bool> MedicalRecordUpdated;

        public UpdateMedicalRecordCommand(UpdateMedicalRecordViewModel viewModel)
        {
            _viewModel = viewModel;
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
        }

        private bool CanExecute(object parameter)
        {
            return !(string.IsNullOrEmpty(_viewModel.FirstName) || string.IsNullOrEmpty(_viewModel.LastName) || _viewModel.Height <= 0 || _viewModel.Weight <= 0);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                ObservableCollection<string> medicalHistory = _viewModel.MedicalHistory;
                ObservableCollection<string> allergies = _viewModel.Allergies;


                MedicalRecord editedMedicalRecord = new MedicalRecord();
                editedMedicalRecord.Height = _viewModel.Height;
                editedMedicalRecord.Weight = _viewModel.Weight;
                editedMedicalRecord.MedicalHistory = medicalHistory.ToList();
                editedMedicalRecord.Allergies = allergies.ToList();

                _patientService.Update(_viewModel.Patient.Username, _viewModel.FirstName, _viewModel.LastName, _viewModel.Patient.Password, _viewModel.Patient.UserStatus, editedMedicalRecord);
                MedicalRecordUpdated?.Invoke(this, true);
            }
        }
    }
}
