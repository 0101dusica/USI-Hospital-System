using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private static List<Appointment> _appointments = new List<Appointment>();
        private const string _storagePath = "../../../../Data/Appointments.json";

        private ISerializer<Appointment> _serializer;

        public AppointmentRepository(ISerializer<Appointment> serializer)
        {
            _serializer = serializer;
            _appointments = Load();
        }

        public List<Appointment> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _appointments);
        }

        public List<Appointment> GetAll()
        {
            return _appointments;
        }

        public Appointment? GetById(string id)
        {
            return _appointments.FirstOrDefault(a => a.Id.Equals(id));
        }

        public void Add(Appointment appointment)
        {
            _appointments.Add(appointment);
            Save();
        }

        public void Delete(Appointment appointment)
        {
            _appointments.Remove(appointment);
            Save();
        }

        public void Update(Appointment updatedAppointment)
        {
            var existingAppointment = GetById(updatedAppointment.Id);
            if (existingAppointment != null)
            {
                existingAppointment.AppointmentType = updatedAppointment.AppointmentType;
                existingAppointment.AppointmentStatus = updatedAppointment.AppointmentStatus;
                existingAppointment.RoomId = updatedAppointment.RoomId;
                existingAppointment.TimeSlot = updatedAppointment.TimeSlot;
                existingAppointment.Anamnesis = updatedAppointment.Anamnesis;
                existingAppointment.DoctorUsername = updatedAppointment.DoctorUsername;
                existingAppointment.PatientUsername = updatedAppointment.PatientUsername;
                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _appointments.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "appointment1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("appointment", ""));
                return $"appointment{lastIdNumber + 1}";
            }
        }

        public List<Appointment> GetSearchAnamnesis(string keyword, Patient patient)
        {
            return new List<Appointment>(
                GetPatientFinishedAppointments(patient)
                    .Where(appointment => appointment.Anamnesis?.Observations?.Contains(keyword, StringComparison.OrdinalIgnoreCase) == true
                                          || appointment.Anamnesis?.Symptoms?.Any(symptom => symptom.Contains(keyword, StringComparison.OrdinalIgnoreCase)) == true));
        }

        public List<Appointment> GetScheduledAppointmentsForPatient(Patient patient)
        {
            return _appointments
                .Where(a => patient.MedicalRecord.AppointmentIds.Contains(a.Id) && a.AppointmentStatus == AppointmentStatus.Scheduled)
                .ToList();
        }

        public List<Appointment> GetPatientFinishedAppointments(Patient patient)
        {
            return _appointments
                .Where(a => patient.MedicalRecord.AppointmentIds.Contains(a.Id) && a.AppointmentStatus == AppointmentStatus.Finished)
                .ToList();
        }

        public List<Appointment> GetDoctorAppointments(Doctor doctor)
        {
            return GetAll().Where(appointment => appointment.DoctorUsername == doctor.Username).ToList();
        }

        public List<Appointment> GetAppointmentsWithinThreeDays(DateTime dateTime, Doctor doctor)
        {
            var threeDaysLater = dateTime.AddDays(3);
            return GetDoctorAppointments(doctor).Where(appointment => appointment.TimeSlot.StartTime >= dateTime && appointment.TimeSlot.StartTime < threeDaysLater).ToList();
        }

        public void CancelAppointment(Appointment appointment)
        {
            var existingAppointment = GetById(appointment.Id);
            if (existingAppointment != null)
            {
                existingAppointment.AppointmentStatus = AppointmentStatus.Canceled;

                Save();
            }
        }

        public bool AreAppointmentsInTimeRangeForRoom(string roomId, TimeSlot timeSlot)
        {
            foreach (Appointment appointment in GetAll())
            {
                if (appointment.RoomId != null)
                {
                    if (appointment.RoomId.Equals(roomId) && appointment.TimeSlot.StartTime >= timeSlot.StartTime && appointment.TimeSlot.EndTime <= timeSlot.EndTime)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void UpdateExamination(string id, string observation, List<string> symptoms, string selectedRoom)
        {
            var existingAppointment = GetById(id);
            if (existingAppointment != null)
            {
                existingAppointment.Anamnesis.Observations = observation;
                existingAppointment.Anamnesis.Symptoms = symptoms;

                existingAppointment.RoomId = selectedRoom;
                existingAppointment.AppointmentStatus = AppointmentStatus.Finished;
                Save();
            }
        }

        public void AddPrescriptionId(Appointment appointment, string id)
        {
            var existingAppointment = GetById(appointment.Id);
            if (existingAppointment != null)
            {
                //existingAppointment.PrescriptionIds.Add(id);
                Save();
            }
        }

        public void UpdateAnamnesisSymptoms(string id, List<string> newSymptoms)
        {
            var existingAppointment = GetById(id);
            if (existingAppointment != null)
            {
                existingAppointment.Anamnesis.Symptoms = newSymptoms;
            }
            Save();
        }

        public Appointment? GetEarliestPatientAppointment(string patientUsername)
        {
            DateTime currentDateTime = DateTime.Now;
            Appointment earliestAppotintment = null;

            return _appointments.FirstOrDefault(appointment =>
           appointment.PatientUsername.Equals(patientUsername) &&
           appointment.TimeSlot.StartTime.Date == currentDateTime.Date &&
           (appointment.TimeSlot.StartTime - currentDateTime).TotalMinutes <= 15 &&
           (appointment.TimeSlot.StartTime - currentDateTime).TotalMinutes >= 0);
        }

        public List<Appointment> GetAppointmentsInNextTwoHours()
        {
            DateTime currentTime = DateTime.Now;
            DateTime twoHoursFromNow = currentTime.AddHours(2);

            List<Appointment> appointments = _appointments
                .Where(appointment =>
                    appointment.TimeSlot.StartTime <= twoHoursFromNow &&
                    appointment.TimeSlot.EndTime >= currentTime)
                .ToList();

            return appointments;
        }

        public int GetAppointmentDuration(AppointmentType appointmentType, string surgeryDuration)
        {
            if (appointmentType == AppointmentType.Surgery)
            {
                return int.Parse(surgeryDuration);
            }
            else if (appointmentType == AppointmentType.Appointment)
            {
                return 15;
            }
            return 0;
        }

        public void ArrangeTakenAppointmentTerms(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor, List<Appointment> appointmentsForAllDoctors, Dictionary<string, string> potentialTakenTerms)
        {

            foreach (Appointment appointment in appointmentsForAllDoctors)
            {
                foreach (Doctor doctor in appointmentsForEachDoctor.Keys)
                {
                    string terms = " ";
                    if (appointmentsForEachDoctor[doctor].Contains(appointment))
                    {
                        terms += doctor.Username + ";" + appointment.TimeSlot.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");
                        potentialTakenTerms.Add(appointment.Id, terms);
                    }
                }
            }

        }

        public void PostponeChoosenAppointment(Appointment postponedAppointment)
        {
            foreach (Appointment appointment in GetAll())
            {
                if (appointment.Id == postponedAppointment.Id)
                {
                    appointment.AppointmentStatus = AppointmentStatus.Canceled;
                    break;
                }
            }
            Save();
        }
        public bool IsAppointmentReadyForStart(Appointment appointment)
        {
            DateTime startTime = appointment.TimeSlot.StartTime.AddMinutes(-15);
            DateTime endTime = appointment.TimeSlot.EndTime;
            DateTime now = DateTime.Now;

            return now >= startTime && now <= endTime;
        }

        public List<Appointment> OrderByDate(ObservableCollection<Appointment>? appointments)
        {
            return new List<Appointment>(appointments!.OrderBy(appointment => appointment.TimeSlot.StartTime));
        }
        public List<Appointment> OrderByDoctor(ObservableCollection<Appointment>? appointments)
        {
            return new List<Appointment>(appointments!.OrderBy(appointment => appointment.TimeSlot.StartTime));
        }
        public List<Appointment> OrderBySpecialization(ObservableCollection<Appointment> appointments, DoctorService doctorService)
        {
            return appointments
                .OrderBy(a => doctorService.GetDoctorSpecialization(a.DoctorUsername))
                .ToList();
        }

        public void ChangeAppointmentRatedStatus(Appointment appointment)
        {
            Appointment? appointmentFromList = GetById(appointment.Id);
            if (appointmentFromList != null)
            {
                appointmentFromList.RatedStatus = RatedStatus.Rated;
                Save();
            }
        }

    }
}
