using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.CommunicatonManagement.Notifications.Model;


namespace ZdravoCorp.CommunicatonManagement.Notifications.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private static List<Notification> _notifications = new List<Notification>();
        private const string _storagePath = "../../../../Data/Notifications.json";

        private ISerializer<Notification> _serializer;


        public NotificationRepository(ISerializer<Notification> serializer)
        {
            _serializer = serializer;
            _notifications = Load();
        }

        public List<Notification> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _notifications);
        }

        public List<Notification> GetAll()
        {
            return _notifications;
        }

        public Notification? GetById(string id)
        {
            return _notifications.FirstOrDefault(n => n.Id.Equals(id));
        }

        public void Add(Notification notification)
        {
            _notifications.Add(notification);
            Save();
        }

        public void Delete(Notification notification)
        {
            _notifications.Remove(notification);
            Save();
        }

        public void Update(Notification updatedNotification)
        {
            var existingNotification = GetById(updatedNotification.Id);
            if (existingNotification != null)
            {
                existingNotification.Username = updatedNotification.Username;
                existingNotification.Title = updatedNotification.Title;
                existingNotification.Description = updatedNotification.Description;
                existingNotification.NotificationStatus = updatedNotification.NotificationStatus;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _notifications.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "notification1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("notification", ""));
                return $"notification{lastIdNumber + 1}";
            }
        }

        public void CreateDoctorNotificationAboutDelay(string doctorUsername, string appointmentId, string StartDate, string StartTime)
        {
            Notification doctorNotification = new Notification();
            doctorNotification.Id = NextId();
            doctorNotification.PersonUsername = doctorUsername;
            doctorNotification.Title = "POSTPONED EXAMINATION DATE";
            doctorNotification.Description = "Delayed appointment: " + appointmentId + "." + " Urgent appointment in the term: " + StartDate + " " + StartTime;
            Add(doctorNotification);
            Save();
        }

        public void CreatePatientNotificationAboutDelay(string patientUsername, string appointmentId, string StartDate, string StartTime)
        {
            Notification patientNotification = new Notification();
            patientNotification.Id = NextId();
            patientNotification.PersonUsername = patientUsername;
            patientNotification.Title = "URGENT APPOINTMENT ";
            patientNotification.Description = "Urgent appointment in the term: " + StartDate + " " + StartTime;
            Add(patientNotification);
            Save();
        }
    }
}
