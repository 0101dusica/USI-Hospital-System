using Newtonsoft.Json;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.SchedulingManagement.Appointments.Model
{
    public enum AppointmentStatus
    {
        Scheduled,
        Canceled,
        Finished

    }

    public enum AppointmentType
    {
        Appointment,
        Surgery,
        EmergencyAppointment,
        EmergencySurgery
    }

    public enum RatedStatus
    {
        Unrated,
        Rated
    }
    public class Appointment
    {
        public string Id { get; set; }
        public string DoctorUsername { get; set; }
        public string PatientUsername { get; set; }
        public AppointmentStatus AppointmentStatus { get; set; }
        public AppointmentType AppointmentType { get; set; }
        public Anamnesis Anamnesis { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string RoomId { get; set; }
        public RatedStatus RatedStatus { get; set; }

        public Appointment() { }

        public Appointment(string id, string doctorUsername, string patientUsername, AppointmentType type, AppointmentStatus status, string roomId, Anamnesis anamnesis, TimeSlot timeSlot)
        {
            Id = id;
            DoctorUsername = doctorUsername;
            PatientUsername = patientUsername;
            AppointmentType = type;
            AppointmentStatus = status;
            RoomId = roomId;
            TimeSlot = timeSlot;
            Anamnesis = anamnesis;

        }

        public Appointment(string id, string doctorUsername, string patientUsername, AppointmentType type, TimeSlot timeSlot)
        {
            Id = id;
            DoctorUsername = doctorUsername;
            PatientUsername = patientUsername;
            AppointmentType = type;
            AppointmentStatus = AppointmentStatus.Scheduled;
            RoomId = "";
            TimeSlot = timeSlot;
            Anamnesis = new Anamnesis();

        }
    }
}
