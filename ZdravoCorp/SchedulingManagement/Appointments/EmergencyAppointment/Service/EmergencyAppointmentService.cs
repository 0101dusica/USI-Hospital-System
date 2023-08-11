using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;

namespace ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Service
{
    public class EmergencyAppointmentService
    {
        private IAppointmentRepository _appointmentRepository;
        public EmergencyAppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public List<Appointment> GetAll()
        {
            return _appointmentRepository.GetAll();
        }

        public Appointment? GetById(string id)
        {
            return _appointmentRepository.GetById(id);
        }

        public void Add(Appointment appointment)
        {
            _appointmentRepository.Add(appointment);
        }

        public string NextId()
        {
            return _appointmentRepository.NextId();
        }


        public Appointment? GetEarliestPatientAppointment(string patientUsername)
        {
            return _appointmentRepository.GetEarliestPatientAppointment(patientUsername);
        }

        public List<Appointment> GetAppointmentsInNextTwoHours()
        {
            return _appointmentRepository.GetAppointmentsInNextTwoHours();
        }

        public Dictionary<Doctor, List<Appointment>> FilterAppointmentsBySpecialization(List<Doctor> specializedDoctors)
        {
            return specializedDoctors.ToDictionary(
                doctor => doctor,
                doctor => GetAppointmentsInNextTwoHours()
                    .Where(appointment => appointment.DoctorUsername.Equals(doctor.Username))
                    .ToList()
            );
        }

        public Dictionary<Doctor, DateTime> GetAppointmentTermForSingleAppointmentDoctor(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor, Doctor doctor, DateTime currentTime, DateTime twoHoursFromNow, int emergencyAppointmentDuration, Dictionary<Doctor, DateTime> earliestTermForEachDoctor)
        {
            foreach (Appointment singleAppointment in appointmentsForEachDoctor[doctor])
            {
                TimeSpan startTimeDifference = singleAppointment.TimeSlot.StartTime - currentTime;
                TimeSpan restTime = twoHoursFromNow - singleAppointment.TimeSlot.EndTime;

                if (startTimeDifference.TotalMinutes >= emergencyAppointmentDuration || restTime.TotalMinutes >= emergencyAppointmentDuration)
                {
                    DateTime potentialAppointmentTerm = startTimeDifference.TotalMinutes >= emergencyAppointmentDuration ? currentTime : singleAppointment.TimeSlot.EndTime;
                    earliestTermForEachDoctor = CalculatePotentialAppointmentTerm(potentialAppointmentTerm, doctor, earliestTermForEachDoctor);
                }
            }

            return earliestTermForEachDoctor;
        }

        public Dictionary<Doctor, DateTime> GetAppointmentTermForEachDoctor(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor, Doctor doctor, DateTime currentTime, DateTime twoHoursFromNow, int emergencyAppointmentDuration, Dictionary<Doctor, DateTime> earliestTermForEachDoctor)
        {
            for (int i = 0; i < appointmentsForEachDoctor[doctor].Count - 1; i++)
            {
                Appointment currentAppointment = appointmentsForEachDoctor[doctor][i];
                Appointment nextAppointment = appointmentsForEachDoctor[doctor][i + 1];

                DateTime currentEndTime = currentAppointment.TimeSlot.EndTime;
                DateTime currentStartTime = currentAppointment.TimeSlot.StartTime;
                DateTime nextStartTime = nextAppointment.TimeSlot.StartTime;
                TimeSpan timeDifference = nextStartTime - currentEndTime;
                TimeSpan startAndFirstTermDifference = currentStartTime - currentTime;
                Console.WriteLine(timeDifference.TotalMinutes);

                if (startAndFirstTermDifference.TotalMinutes >= emergencyAppointmentDuration)
                {
                    earliestTermForEachDoctor = CalculatePotentialAppointmentTerm(currentTime, doctor, earliestTermForEachDoctor);
                }
                if (timeDifference.TotalMinutes >= emergencyAppointmentDuration)
                {
                    earliestTermForEachDoctor = CalculatePotentialAppointmentTerm(currentEndTime, doctor, earliestTermForEachDoctor);
                }
                else if (nextAppointment == appointmentsForEachDoctor[doctor][appointmentsForEachDoctor[doctor].Count - 1])
                {
                    TimeSpan restTime = twoHoursFromNow - nextAppointment.TimeSlot.EndTime;
                    if (restTime.TotalMinutes >= emergencyAppointmentDuration)
                    {
                        earliestTermForEachDoctor = CalculatePotentialAppointmentTerm(nextAppointment.TimeSlot.EndTime, doctor, earliestTermForEachDoctor);
                    }
                }
            }
            return earliestTermForEachDoctor;
        }

        public Dictionary<Doctor, DateTime> CalculatePotentialAppointmentTerm(DateTime option, Doctor doctor, Dictionary<Doctor, DateTime> earliestTermForEachDoctor)
        {
            DateTime potentialTerm = option;
            earliestTermForEachDoctor.Add(doctor, potentialTerm);
            return earliestTermForEachDoctor;
        }

        public Dictionary<Doctor, DateTime> SortAppointmentTermsForEachDoctor(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor, DateTime currentTime, DateTime twoHoursFromNow, int emergencyAppointmentDuration)
        {
            Dictionary<Doctor, DateTime> earliestTermForEachDoctor = new Dictionary<Doctor, DateTime>();
            foreach (Doctor doctor in appointmentsForEachDoctor.Keys)
            {
                if (appointmentsForEachDoctor[doctor].Count == 1)
                {
                    earliestTermForEachDoctor = GetAppointmentTermForSingleAppointmentDoctor(appointmentsForEachDoctor, doctor, currentTime, twoHoursFromNow, emergencyAppointmentDuration, earliestTermForEachDoctor);
                }
                else
                {
                    appointmentsForEachDoctor[doctor].Sort((a1, a2) => a1.TimeSlot.StartTime.CompareTo(a2.TimeSlot.StartTime));
                    earliestTermForEachDoctor = GetAppointmentTermForEachDoctor(appointmentsForEachDoctor, doctor, currentTime, twoHoursFromNow, emergencyAppointmentDuration, earliestTermForEachDoctor);
                }
            }
            return earliestTermForEachDoctor;
        }

        public Dictionary<string, string> SortPotentialTakenTerms(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor)
        {
            Dictionary<string, string> potentialTakenTerms = new Dictionary<string, string>();
            List<Appointment> appointmentsForAllDoctors = new List<Appointment>();

            foreach (Doctor doctor in appointmentsForEachDoctor.Keys)
            {
                foreach (Appointment appointment in appointmentsForEachDoctor[doctor])
                {
                    appointmentsForAllDoctors.Add(appointment);
                }
            }
            appointmentsForAllDoctors.Sort((a1, a2) => a1.TimeSlot.StartTime.CompareTo(a2.TimeSlot.StartTime));

            foreach (Appointment appointment in appointmentsForAllDoctors)
            {
                Console.WriteLine(appointment.TimeSlot.StartTime);
            }

            if (appointmentsForAllDoctors.Count >= 5)
            {
                var firstFiveItems = appointmentsForAllDoctors.Take(5).ToList();
                ArrangeTakenAppointmentTerms(appointmentsForEachDoctor, firstFiveItems, potentialTakenTerms);

            }
            else
            {
                ArrangeTakenAppointmentTerms(appointmentsForEachDoctor, appointmentsForAllDoctors, potentialTakenTerms);
            }
            return potentialTakenTerms;
        }

        public void ArrangeTakenAppointmentTerms(Dictionary<Doctor, List<Appointment>> appointmentsForEachDoctor, List<Appointment> appointmentsForAllDoctors, Dictionary<string, string> potentialTakenTerms)
        {
            DateTime currentTime = DateTime.Now;
            currentTime = currentTime.AddSeconds(-currentTime.Second);
            foreach (Appointment appointment in appointmentsForAllDoctors)
            {
                foreach (Doctor doctor in appointmentsForEachDoctor.Keys)
                {
                    string terms = " ";
                    if (appointmentsForEachDoctor[doctor].Contains(appointment))
                    {
                        if (DateTime.Compare(appointment.TimeSlot.StartTime, DateTime.Now) < 0)
                        {
                            appointment.TimeSlot.StartTime = currentTime;
                        }
                        terms += doctor.Username + ";" + appointment.TimeSlot.StartTime.ToString("yyyy-MM-ddTHH:mm:ss");

                        potentialTakenTerms.Add(appointment.Id, terms);
                    }
                }
            }

        }

        public void PostponeChoosenAppointment(Appointment postponedAppointment)
        {
            _appointmentRepository.PostponeChoosenAppointment(postponedAppointment);
        }
    }
}
