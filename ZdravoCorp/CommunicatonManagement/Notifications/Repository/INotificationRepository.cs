using System.Collections.Generic;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;

namespace ZdravoCorp.CommunicatonManagement.Notifications.Repository
{
    public interface INotificationRepository
    {
        List<Notification> GetAll();
        Notification? GetById(string id);
        void Add(Notification notification);
        void Delete(Notification notification);
        void Update(Notification updatedNotification);
        string NextId();
        void CreateDoctorNotificationAboutDelay(string doctorUsername, string appointmentId, string StartDate, string StartTime);
        void CreatePatientNotificationAboutDelay(string patientUsername, string appointmentId, string StartDate, string StartTime);
    }
}
