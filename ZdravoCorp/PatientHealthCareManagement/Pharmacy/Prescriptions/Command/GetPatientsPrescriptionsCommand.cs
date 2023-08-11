using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Command
{
    public class GetPatientsPrescriptionsCommand : BaseCommand
    {
        private PrescriptionService _prescriptionService;
        private NurseMedicationDispenserViewModel _viewModel;

        public GetPatientsPrescriptionsCommand(NurseMedicationDispenserViewModel viewModel)
        {
            _prescriptionService = new PrescriptionService(new PrescriptionRepository(new Serializer<Prescription>()));
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.isPatientSelected();
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Patient selectedPatient = _viewModel.SelectedPatient;
                _viewModel.PrescriptionTable.Clear();
                var prescriptions = _prescriptionService.GetPrescriptionsEndingByTomorrow(selectedPatient.Username);
                if (prescriptions.Count == 0)
                {
                    MessageBox.Show($"The patient has no prescriptions that end tomorrow at the latest!");
                }
                else
                {
                    foreach (var prescription in prescriptions)
                    {
                        _viewModel.PrescriptionTable.Add(prescription);
                    }
                }

            }
        }
    }
}
