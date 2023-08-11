using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Notifications.Repository;
using ZdravoCorp.CommunicatonManagement.Notifications.ViewModel;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.CommunicatonManagement.Notifications.Service
{
    public class NotificationService
    {
        private INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public List<Notification> GetAll()
        {
            return _notificationRepository.GetAll();
        }

        public Notification? GetById(string id)
        {
            return _notificationRepository.GetById(id);
        }

        public void Add(Notification notification)
        {
            _notificationRepository.Add(notification);

        }

        public void Delete(Notification notification)
        {
            _notificationRepository.Delete(notification);
        }

        public void Update(Notification updatedNotification)
        {
            _notificationRepository.Update(updatedNotification);
        }

        public string NextId()
        {
            return _notificationRepository.NextId();
        }

        public void CreateDoctorNotificationAboutDelay(string doctorUsername, string appointmentId, string StartDate,
            string StartTime)
        {
            _notificationRepository.CreateDoctorNotificationAboutDelay(doctorUsername, appointmentId, StartDate,
                StartTime);
        }

        public void CreatePatientNotificationAboutDelay(string patientUsername, string appointmentId, string StartDate,
            string StartTime)
        {
            _notificationRepository.CreatePatientNotificationAboutDelay(patientUsername, appointmentId, StartDate,
                StartTime);
        }

        internal void CreateNotificationAboutPatient(PatientNotificationViewModel notificationViewModel,
            Patient patient)
        {
            DateTime startTime = DateTime.Parse(notificationViewModel.StartDate!);
            DateTime endTime = DateTime.Parse(notificationViewModel.StartDate!);
            TimeSpan timeSpan = TimeSpan.Parse(notificationViewModel.Time!);
            Notification notification = new Notification(NextId(), patient.Username, notificationViewModel.Title!,
                notificationViewModel.Description!, new TimeSlot(startTime, endTime), timeSpan);
            Add(notification);

        }
    }
}
