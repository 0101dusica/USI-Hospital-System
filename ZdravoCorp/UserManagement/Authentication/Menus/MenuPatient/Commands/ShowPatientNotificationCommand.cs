using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;
using ZdravoCorp.CommunicatonManagement.Notifications.Repository;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.CommunicatonManagement.Notifications.View;
using ZdravoCorp.CommunicatonManagement.Notifications.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands
{
    public class ShowPatientNotificationCommand : BaseCommand
    {
        private NotificationService _notificationService;
        private PrescriptionService _prescriptionService;
        private MedicineService _medicineService;
        private PatientNotificationService _patientNotificationService;
        private Patient _patient;

        public ShowPatientNotificationCommand(PatientNotificationService patientNotificationService, Patient patient)
        {
            _notificationService = new NotificationService(new NotificationRepository(new Serializer<Notification>()));
            _prescriptionService = new PrescriptionService(new PrescriptionRepository(new Serializer<Prescription>()));
            _medicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));
            _patientNotificationService = patientNotificationService;
            _patient = patient;
        }



        public override void Execute(object? parameter)
        {
            PatientNotificationsView patientNotificationsView = new PatientNotificationsView();
            patientNotificationsView.DataContext = new PatientNotificationViewModel(_notificationService, _prescriptionService, _medicineService, _patientNotificationService, _patient);
            patientNotificationsView.ShowDialog();
        }
    }
}
