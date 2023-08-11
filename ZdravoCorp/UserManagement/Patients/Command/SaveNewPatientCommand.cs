using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Patients.Command
{
    public class SaveNewPatientCommand : BaseCommand
    {
        private PatientService _patientsService;
        private NursePatientsListViewModel _viewModel;
        private InsertMedicalRecordView _insertMedicalRecordView;
        private InsertMedicalRecordViewModel _insertMedicalRecordViewModel;
        private Patient _newPatient;


        public SaveNewPatientCommand(InsertMedicalRecordView insertMedicalRecordView, InsertMedicalRecordViewModel insertMedicalRecordViewModel, Patient newPatient, NursePatientsListViewModel nursePatientsListViewModel)
        {
            _patientsService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _insertMedicalRecordView = insertMedicalRecordView;
            _insertMedicalRecordViewModel = insertMedicalRecordViewModel;
            _viewModel = nursePatientsListViewModel;
            _newPatient = newPatient;
        }

        public override void Execute(object? parameter)
        {
            int height = int.Parse(_insertMedicalRecordViewModel.Height);
            int weight = int.Parse(_insertMedicalRecordViewModel.Weight);
            List<string> medicalHistoryList = new List<string>();
            if (!string.IsNullOrEmpty(_insertMedicalRecordViewModel.MedicalHistory))
            {
                string[] medicalHistoryArray = _insertMedicalRecordViewModel.MedicalHistory.Split(',');
                medicalHistoryList = medicalHistoryArray.ToList();
            }
            _newPatient.MedicalRecord.Height = height;
            _newPatient.MedicalRecord.Weight = weight;
            _newPatient.MedicalRecord.MedicalHistory = medicalHistoryList;
            _patientsService.Add(_newPatient);

            _insertMedicalRecordView.Close();

            _viewModel.PatientsTable = _viewModel.LoadPatients();


        }
    }
}
