using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.CommunicatonManagement.Notifications.Model
{
    public enum NotificationStatus
    {
        NotShowed,
        Showed
    }
    public class Notification
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public NotificationStatus NotificationStatus { get; set; }
        public string PersonUsername { get; internal set; }

        public TimeSlot TimeSlot { get; set; }
        public TimeSpan Time { get; set; }


        public Notification() { }

        public Notification(string id, string username, string title, string description, TimeSlot timeSlot,
            TimeSpan time)
        {
            Id = id;
            Username = username;
            Title = title;
            Description = description;
            NotificationStatus = NotificationStatus.NotShowed;
            TimeSlot = timeSlot;
            Time = time;

        }

    }
}
