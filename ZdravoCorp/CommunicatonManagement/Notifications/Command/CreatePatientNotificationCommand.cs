using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.CommunicatonManagement.Notifications.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.CommunicatonManagement.Notifications.Command
{
    public class CreatePatientNotificationCommand : BaseCommand
    {
        private NotificationService _notificationService;
        private PatientNotificationViewModel _notificationViewModel;
        private Patient _patient;
        public CreatePatientNotificationCommand(PatientNotificationViewModel patientNotificationViewModel, Patient patient, NotificationService notificationService)
        {
            _notificationService = notificationService;
            _notificationViewModel = patientNotificationViewModel;
            _patient = patient;
        }
        public override void Execute(object? parameter)
        {
            if (_notificationViewModel.ValidateInputs())
            {
                _notificationService.CreateNotificationAboutPatient(_notificationViewModel, _patient);
                MessageBox.Show("New notification added!");
                _notificationViewModel.LoadDataGrid(_notificationViewModel.PrescriptionService);

            }
        }
    }
}
