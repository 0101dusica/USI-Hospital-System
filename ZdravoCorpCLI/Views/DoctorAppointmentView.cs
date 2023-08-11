using System.Collections.ObjectModel;
using System.Globalization;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorpCLI.Views
{
    public class DoctorAppointmentView
    {
        private AppointmentService _appointmentService;
        private PatientService _patientService;

        private Doctor _loggedDoctor;
        private List<Appointment> _appointments;

        public DoctorAppointmentView(Doctor doctor)
        {
            _loggedDoctor = doctor;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));

            _appointments = _appointmentService.GetDoctorAppointments(_loggedDoctor);

            ShowMenu();
        }

        public void ShowMenu()
        {
            PrintAppointments(_appointments);

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Options:");
                Console.WriteLine("1. Show Medical Record");
                Console.WriteLine("2. Add Appointment");
                Console.WriteLine("3. Update Appointment");
                Console.WriteLine("4. Cancel Appointment");
                Console.WriteLine("5. Show Appointments for Next 3 Days");
                Console.WriteLine("6. Exit");

                Console.WriteLine();
                Console.Write("Enter option: ");
                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        ShowMedicalRecord();
                        break;
                    case "2":
                        AddAppointment();
                        break;
                    case "3":
                        UpdateAppointment();
                        break;
                    case "4":
                        CancelAppointment();
                        break;
                    case "5":
                        ShowAppointmentsForNext3Days();
                        break;
                    case "6":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                PrintAppointments(_appointments);
            }
        }

        private void ShowAppointmentsForNext3Days()
        {
            Console.WriteLine("Show Appointments for Next 3 Days");

            Console.Write("Enter date (dd.MM.yyyy): ");
            string dateInput = Console.ReadLine()!;
            if (!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime selectedDate))
            {
                Console.WriteLine("Invalid date format. Please enter the date in the format dd.MM.yyyy.");
                return;
            }

            List<Appointment> appointmentsWithinThreeDays = _appointmentService.GetAppointmentsWithinThreeDays(selectedDate, _loggedDoctor);
            PrintAppointments(appointmentsWithinThreeDays);
        }

        private void AddAppointment()
        {
            Console.WriteLine("Add Appointment");

            PrintPatients();

            Console.Write("Enter patient ID: ");
            string patientUsername = Console.ReadLine()!;

            Patient patient = _patientService.GetByUsername(patientUsername)!;
            if (patient == null)
            {
                Console.WriteLine("Invalid patient ID. Please try again.");
                return;
            }

            Console.Write("Enter appointment start time (dd.MM.yyyy HH:mm): ");
            string startTimeInput = Console.ReadLine()!;
            if (!DateTime.TryParseExact(startTimeInput, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
            {
                Console.WriteLine("Invalid date format. Please enter the date and time in the format dd.MM.yyyy HH:mm.");
                return;
            }

            if (startTime < DateTime.Now)
            {
                Console.WriteLine("Appointment start time cannot be in the past. Please enter a future date and time.");
                return;
            }

            Console.WriteLine("Appointment types:");
            foreach (AppointmentType type in Enum.GetValues(typeof(AppointmentType)))
            {
                Console.WriteLine($"{(int)type}. {type}");
            }

            Console.Write("Enter appointment type (number): ");
            if (!int.TryParse(Console.ReadLine(), out int appointmentTypeValue))
            {
                Console.WriteLine("Invalid appointment type. Please enter a valid number.");
                return;
            }

            if (!Enum.IsDefined(typeof(AppointmentType), appointmentTypeValue))
            {
                Console.WriteLine("Invalid appointment type. Please enter a valid number.");
                return;
            }

            AppointmentType appointmentType = (AppointmentType)appointmentTypeValue;

            Console.Write("Enter duration (minutes): ");
            if (!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)
            {
                Console.WriteLine("Invalid duration. Please enter a positive integer value.");
                return;
            }

            if (appointmentType == AppointmentType.Appointment && duration != 15)
            {
                Console.WriteLine("Invalid duration. Appointment type 'Appointment' requires a duration of 15 minutes.");
                return;
            }

            Appointment newAppointment = new Appointment(_appointmentService.NextId(), _loggedDoctor.Username, patientUsername, appointmentType, new TimeSlot(startTime, startTime.AddMinutes(duration)));
            try
            {
                _appointmentService.CreateAppointmentDoctor(newAppointment);
                Console.WriteLine("Appointment added successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to schedule the appointment. The requested time slot is not available.");
            }

            PrintAppointments(_appointments);
        }




        void ShowMedicalRecord()
        {
            Console.Write("Enter appointment ID: ");
            string appointmentId = Console.ReadLine()!;

            Appointment? appointment = _appointmentService.GetById(appointmentId!);
            if (appointment != null)
            {

                Console.WriteLine("Showing medical record for appointment ID: " + appointment.Id + "\n");

                Patient? patient = _patientService.GetByUsername(appointment.PatientUsername);
                if (patient != null)
                {
                    Console.WriteLine("First Name: " + patient.FirstName);
                    Console.WriteLine("Last Name: " + patient.LastName);
                    Console.WriteLine("Height: " + patient.MedicalRecord.Height);
                    Console.WriteLine("Weight: " + patient.MedicalRecord.Weight);
                    Console.WriteLine("Allergies:");
                    foreach (string allergy in patient.MedicalRecord.Allergies)
                    {
                        Console.WriteLine("- " + allergy);
                    }
                    Console.WriteLine("Medical History:");
                    foreach (string history in patient.MedicalRecord.MedicalHistory)
                    {
                        Console.WriteLine("- " + history);
                    }
                }
                else
                {
                    Console.WriteLine("Patient not found.");
                }
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }


        void CancelAppointment()
        {
            Console.Write("Enter appointment ID: ");
            string appointmentId = Console.ReadLine()!;

            Appointment? appointment = _appointmentService.GetById(appointmentId!);
            if (appointment != null)
            {
                if (appointment.AppointmentStatus == AppointmentStatus.Scheduled)
                {
                    _appointmentService.CancelAppointment(appointment);
                    Console.WriteLine("Cancelling appointment with ID: " + appointment.Id);
                    appointment.AppointmentStatus = AppointmentStatus.Canceled;

                    PrintAppointments(_appointments);
                }
                else
                {
                    Console.WriteLine("Unable to cancel appointment. The appointment is not in the scheduled state.");
                }
            }
            else
            {
                Console.WriteLine("Appointment not found.");
            }
        }

        private void UpdateAppointment()
        {
            Console.WriteLine("Update Appointment");

            PrintAppointments(_appointments);

            Console.Write("Enter appointment ID: ");
            string appointmentId = Console.ReadLine()!;

            Appointment? appointment = _appointmentService.GetById(appointmentId);
            if (appointment == null)
            {
                Console.WriteLine("Appointment not found.");
                return;
            }

            if (appointment.AppointmentStatus != AppointmentStatus.Scheduled)
            {
                Console.WriteLine("Unable to update appointment. The appointment is not in the scheduled state.");
                return;
            }

            try
            {
                Console.Write("Enter appointment start time (dd.MM.yyyy HH:mm): ");
                string startTimeInput = Console.ReadLine()!;
                if (!DateTime.TryParseExact(startTimeInput, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    Console.WriteLine("Invalid date format. Please enter the date and time in the format dd.MM.yyyy HH:mm.");
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    Console.WriteLine("Appointment start time cannot be in the past. Please enter a future date and time.");
                    return;
                }

                Console.Write("Enter new duration (minutes): ");
                if (!int.TryParse(Console.ReadLine(), out int duration) || duration <= 0)
                {
                    Console.WriteLine("Invalid duration. Please enter a positive integer value.");
                    return;
                }

                Appointment updatedAppointment = new Appointment(
                    appointment.Id,
                    appointment.DoctorUsername,
                    appointment.PatientUsername,
                    appointment.AppointmentType,
                    new TimeSlot(startTime, startTime.AddMinutes(duration))
                );

                _appointmentService.TryUpdate(updatedAppointment, _loggedDoctor, _loggedDoctor, startTime, startTime.AddMinutes(duration), _patientService.GetByUsername(appointment.PatientUsername)!);

                Console.WriteLine("Appointment updated successfully.");
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to update the appointment. The requested time slot is not available.");
            }
        }


        void PrintPatients()
        {
            Console.WriteLine();
            Console.WriteLine("Patients:");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("|    Id          |   First Name             |   Last Name              |");
            Console.WriteLine("-------------------------------------------------------------------");

            foreach (var patient in _patientService.GetAll())
            {
                Console.WriteLine($"| {patient.Username,-14} | {patient.FirstName,-24} | {patient.LastName,-24} |");
                Console.WriteLine("-------------------------------------------------------------------");
            }

            Console.WriteLine();
        }



        void PrintAppointments(List<Appointment> appointments)
        {
            Console.WriteLine();
            Console.WriteLine("Doctor Appointments:");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("|    Id          |   Patient       |     Start                |      End                 | Type         | Status       |");
            Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");

            foreach (var appointment in appointments)
            {
                Console.WriteLine($"| {appointment.Id,-14} | {appointment.PatientUsername,-15} | {appointment.TimeSlot.StartTime,-24} | {appointment.TimeSlot.EndTime,-24} | {appointment.AppointmentType,-12} | {appointment.AppointmentStatus,-12} |");
                Console.WriteLine("------------------------------------------------------------------------------------------------------------------------");
            }

            Console.WriteLine();
        }



    }
}
