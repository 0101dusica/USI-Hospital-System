using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.CommunicatonManagement.Notifications.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.CommunicatonManagement.Notifications.Command
{
    public class ChangePrescriptionNotificationCommand : BaseCommand
    {
        private PatientNotificationViewModel _notificationViewModel;
        private PrescriptionService _prescriptionService;
        public ChangePrescriptionNotificationCommand(PatientNotificationViewModel patientNotificationViewModel, Patient loggedPatient, PrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
            _notificationViewModel = patientNotificationViewModel;
        }

        public override void Execute(object? parameter)
        {
            if (_notificationViewModel.SelectedPrescription != null)
            {
                if (!_notificationViewModel.ValidateTimeSet()) return;
                Prescription prescription = _notificationViewModel.SelectedPrescription;
                prescription.TimeSet = int.Parse(_notificationViewModel.TimeSet!);
                _prescriptionService.Update(prescription);
                _notificationViewModel.SelectedPrescription = null;
                _notificationViewModel.LoadDataGrid(_notificationViewModel.PrescriptionService);
            }
            else
            {
                MessageBox.Show("Row must be selected!");
            }
        }
    }
}
