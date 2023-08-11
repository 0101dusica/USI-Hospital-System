using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.UserManagement.Patients.Command;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel
{
    public class InsertMedicalRecordViewModel : BaseViewModel
    {
        //Bindings
        private NursePatientsListViewModel _viewModel;
        private InsertMedicalRecordView _insertMedicalRecordView { get; set; }
        private Patient _newPatient;

        public ICommand SaveNewPatientCommand { get; set; }

        private string _weight;
        public string Weight
        {
            get { return _weight; }
            set
            {
                _weight = value;
                OnPropertyChanged(nameof(_weight));
            }
        }

        private string _height;
        public string Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }

        private string _medicalHistory;
        public string MedicalHistory
        {
            get { return _medicalHistory; }
            set
            {
                _medicalHistory = value;
                OnPropertyChanged(nameof(MedicalHistory));
            }
        }

        //Constructor
        public InsertMedicalRecordViewModel(InsertMedicalRecordView insertMedicalRecordView, Patient newPatient, NursePatientsListViewModel nursePatientsListViewModel)
        {
            _newPatient = newPatient;
            _insertMedicalRecordView = insertMedicalRecordView;
            _viewModel = nursePatientsListViewModel;

            SaveNewPatientCommand = new SaveNewPatientCommand(_insertMedicalRecordView, this, _newPatient, _viewModel);

        }

        //Validations
        public bool isInputForMedicalRecordEmpty(string height, string weight, string medicalHistory)
        {
            int number;
            bool isHeightInteger = int.TryParse(height, out number);
            bool isWeightInteger = int.TryParse(weight, out number);
            if (string.IsNullOrEmpty(height) || string.IsNullOrEmpty(weight) || string.IsNullOrEmpty(medicalHistory))
            {
                MessageBox.Show($"Cannot submit an empty form!");
                return false;
            }
            else if (!isHeightInteger || !isWeightInteger)
            {
                MessageBox.Show($"Height and weight must be integers!");
                return false;
            }

            return true;
        }

    }
}
