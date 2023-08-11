using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.SchedulingManagement.FreeDays.Model
{
    public enum RequestStatus
    {
        InProccess,
        Accepted,
        Rejected
    }

    public class FreeDay
    {
        public string Id { get; set; }

        public string DoctorUsername { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public string Description { get; set; }
        public TimeSlot TimeSlot { get; set; }

        public FreeDay() { }

        public FreeDay(string id, string doctorUsername, RequestStatus statusRequest, string description, TimeSlot timeSlot)
        {
            Id = id;
            Description = description;
            RequestStatus = statusRequest;
            TimeSlot = timeSlot;
            DoctorUsername = doctorUsername;

        }
    }
}
