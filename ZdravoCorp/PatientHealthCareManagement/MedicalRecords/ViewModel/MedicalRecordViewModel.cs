using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel
{
    public class MedicalRecordViewModel : BaseViewModel
    {
        private PatientService _patientService;
        public Patient Patient { get; set; }
        public MedicalRecordViewModel(string patientUsername)
        {
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            Patient = _patientService.GetByUsername(patientUsername);
        }
    }
}
