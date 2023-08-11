using ZdravoCorp.CommunicatonManagement.Notifications.Model;
using ZdravoCorp.CommunicatonManagement.Notifications.Repository;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Service;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorpCLI.Views
{
    public class EmergencyAppointmentView
    {
        private PatientService _patientService;
        private DoctorService _doctorService;
        private EmergencyAppointmentService _emergencyAppointmentService;
        private NotificationService _notificationService;

        private Patient _choosenPatient;
        private string _choosenSpecialization = "";
        private Doctor _choosenDoctor;
        private DateTime startDate;
        private DateTime endDate;
        private int appointmentDuration;
        private AppointmentType appointmentType;


        private List<Doctor> _doctorsWithoutAppointments = new List<Doctor>();
        public Dictionary<string, string> potentialTakenTerms = new Dictionary<string, string>();
        private Dictionary<Doctor, DateTime> _earliestTermForEachDoctor = new Dictionary<Doctor, DateTime>();
        private Dictionary<Doctor, List<Appointment>> _appointmentsForEachDoctor = new Dictionary<Doctor, List<Appointment>>();

        private DateTime currentDateTime;
        private DateTime twoHoursFromNow;


        public EmergencyAppointmentView()
        {
            currentDateTime = DateTime.Now;
            twoHoursFromNow = currentDateTime.AddHours(2);

            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _emergencyAppointmentService = new EmergencyAppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _notificationService = new NotificationService(new NotificationRepository(new Serializer<Notification>()));

            Run();
        }

        public void Run()
        {
            _choosenPatient = ChoosePatient();
            _choosenSpecialization = ChooseDoctorSpecialization();
            ChooseAppointmentType();
            _doctorsWithoutAppointments = GetDoctorsWithoutAppointments();
            _appointmentsForEachDoctor = FindAppointmentsForEeachDoctor();
            _earliestTermForEachDoctor = FindEarliestTermForEachDoctor();
            GetEmergencyAppointmentData();
        }

        public Patient ChoosePatient()
        {

            List<Patient> patients = _patientService.GetAll();

            Console.WriteLine("\nPATIENTS TABLE\n");
            Console.WriteLine("======================================================================");
            Console.WriteLine("|  First Name      |   Last Name      |  Username    |");
            Console.WriteLine("=======================================================================");

            foreach (Patient patient in patients)
            {
                Console.WriteLine($"|  {patient.FirstName,-12} |   {patient.LastName,-12} |  {patient.Username,-4}       |");
            }

            Console.WriteLine("=======================================================================");

            bool inputIsValid = false;
            string username = "";
            while (!inputIsValid)
            {
                Console.WriteLine("\nChoose patient (enter username): ");
                username = Console.ReadLine();
                Patient patient = _patientService.GetByUsername(username);

                if (!patients.Contains(patient))
                {
                    Console.WriteLine("\nInput patient doesn't exists!");
                }
                else
                {
                    inputIsValid = true;
                }
            }




            return _patientService.GetByUsername(username);

        }

        public string ChooseDoctorSpecialization()
        {
            List<string> specializations = _doctorService.GetAllSpecializations();

            Console.WriteLine("\nSPECIALIZATIONS TABLE\n");
            Console.WriteLine("===================================");
            Console.WriteLine("|   Specialization   |");
            Console.WriteLine("===================================");

            foreach (string specialization in specializations)
            {
                Console.WriteLine($"|   {specialization,-16} |");
            }
            Console.WriteLine("===================================");
            string choosenSpecialization = "";
            bool inputIsValid = false;
            while (!inputIsValid)
            {
                Console.WriteLine("\nChoose doctor specialization: ");

                choosenSpecialization = Console.ReadLine();

                if (!specializations.Contains(choosenSpecialization))
                {
                    Console.WriteLine("\nUnvalid specialization!");
                }
                else
                {
                    inputIsValid = true;
                }
            }


            return choosenSpecialization;
        }

        public int ChooseAppointmentType()
        {

            bool inputIsValid = false;

            while (!inputIsValid)
            {
                Console.WriteLine("\nChoose appointment type: ");
                Console.WriteLine("1.Appointment");
                Console.WriteLine("2.Operation");

                string option = "";
                option = Console.ReadLine();

                if (option.Equals("1"))
                {
                    appointmentDuration = 15;
                    appointmentType = AppointmentType.Appointment;

                    inputIsValid = true;
                }
                else if (option.Equals("2"))
                {
                    string duration = "";
                    appointmentType = AppointmentType.Surgery;
                    Console.WriteLine("\nEnter operation duration: ");
                    duration = Console.ReadLine();
                    appointmentDuration = int.Parse(duration);

                    inputIsValid = true;
                }
                else
                {
                    Console.WriteLine("\nUnvalid option!");
                }
            }


            return appointmentDuration;

        }

        public List<Doctor> GetSpecializedDoctors(string choosenSpecialization)
        {
            List<Doctor> specializedDoctors = _doctorService.FilterBySpecialization(choosenSpecialization);
            return specializedDoctors;
        }

        public List<Doctor> GetDoctorsWithoutAppointments()
        {
            return _doctorService.GetDoctorWithoutAppointments();
        }

        public Dictionary<Doctor, List<Appointment>> FindAppointmentsForEeachDoctor()
        {
            List<Doctor> specializedDoctors = GetSpecializedDoctors(_choosenSpecialization);
            return _emergencyAppointmentService.FilterAppointmentsBySpecialization(specializedDoctors);
        }

        public Dictionary<Doctor, DateTime> FindEarliestTermForEachDoctor()
        {
            return _emergencyAppointmentService.SortAppointmentTermsForEachDoctor(_appointmentsForEachDoctor, currentDateTime, twoHoursFromNow, appointmentDuration);
        }

        public void GetEmergencyAppointmentData()
        {
            DateTime currentDateTime = DateTime.Now;
            DateTime twoHoursFromNow = currentDateTime.AddHours(2);

            if (_doctorsWithoutAppointments.Count != 0)
            {
                _choosenDoctor = _doctorsWithoutAppointments[0];
                startDate = currentDateTime;
                endDate = currentDateTime.AddMinutes(appointmentDuration);

                Console.WriteLine("\nDoctor: " + _choosenDoctor.Username);
                Console.WriteLine("Start date and time: " + startDate.ToString("dd.MM.yyyy HH:mm"));

                CreateAppointment();
            }
            else
            {

                _choosenDoctor = _doctorService.FindDoctorWithEarliestTerm(_earliestTermForEachDoctor);
                DateTime emergencyAppointmentDate = DateTime.MinValue;
                if (_choosenDoctor != null)
                {
                    emergencyAppointmentDate = _doctorService.FindDoctorsEarliestTerm(_earliestTermForEachDoctor, _choosenDoctor.Username);
                }

                if (_choosenDoctor != null && emergencyAppointmentDate != DateTime.MinValue)
                {
                    startDate = emergencyAppointmentDate;
                    endDate = emergencyAppointmentDate.AddMinutes(appointmentDuration);
                    TimeSlot timeSlot = new TimeSlot(startDate, endDate);

                    Console.WriteLine(_choosenDoctor.Username);
                    Console.WriteLine(emergencyAppointmentDate.ToString("dd.MM.yyyy HH:mm"));

                    CreateAppointment();
                }
                else
                {
                    ChooseAppointmentToPostpone();
                }

            }
        }

        public void ChooseAppointmentToPostpone()
        {
            potentialTakenTerms = _emergencyAppointmentService.SortPotentialTakenTerms(_appointmentsForEachDoctor);

            Console.WriteLine("\nTAKEN TERMS TABLE\n");

            Console.WriteLine("=============================================================================");
            Console.WriteLine("|   Appointment ID   |   Doctor   |   Date and Time        |");
            Console.WriteLine("=============================================================================");

            foreach (var term in potentialTakenTerms)
            {
                string[] appointmentData = term.Value.Split(';');
                string doctorUsername = appointmentData[0];
                string appointmentDateTime = appointmentData[1];

                Console.WriteLine($"|   {term.Key,-17} |   {doctorUsername,-10} |   {appointmentDateTime,-12} |");
            }

            Console.WriteLine("=============================================================================");

            bool isInputValid = false;
            string appointmentId = "";
            while (!isInputValid)
            {
                Console.WriteLine("\nEnter appointment id: ");
                appointmentId = Console.ReadLine();

                if (!potentialTakenTerms.ContainsKey(appointmentId))
                {
                    Console.WriteLine("\nInvalid appointment id!");
                }
                else
                {
                    isInputValid = true;
                }
            }

            Appointment postoponedAppointment = _emergencyAppointmentService.GetById(appointmentId);

            startDate = postoponedAppointment.TimeSlot.StartTime;
            endDate = startDate.AddMinutes(appointmentDuration);

            //_emergencyAppointmentService.PostponeChoosenAppointment(postoponedAppointment);

            Console.WriteLine("\nSuccessfuly postponed appointment " + postoponedAppointment.Id);
            //_notificationService.CreateDoctorNotificationAboutDelay(_choosenDoctor.Username, postoponedAppointment.Id, startDate.ToString("dd.MM.yyyy HH:mm"), endDate.ToString("dd.MM.yyyy HH:mm"));

            CreateAppointment();
        }


        public void CreateAppointment()
        {
            TimeSlot timeSlot = new TimeSlot(startDate, endDate);
            Appointment emergencyAppointment = new Appointment(_emergencyAppointmentService.NextId(), _choosenDoctor.Username, _choosenPatient.Username, appointmentType, timeSlot);
            //_emergencyAppointmentService.Add(emergencyAppointment);

            Console.WriteLine("\nSuccessfuly created emergency appointment " + emergencyAppointment.Id);

            //_notificationService.CreatePatientNotificationAboutDelay(_choosenPatient.Username, emergencyAppointment.Id, startDate.ToString("dd.MM.yyyy HH:mm"), endDate.ToString("dd.MM.yyyy HH:mm"));

            ShowEmergencyAppointment(emergencyAppointment);
        }

        public void ShowEmergencyAppointment(Appointment emergencyAppointment)
        {
            Console.WriteLine("==============================================================================================================");
            Console.WriteLine("|Appointment ID  |Doctor Username  |Patient Username  |Appointment Type  |Start time      |End time      |");
            Console.WriteLine("==============================================================================================================");
            Console.WriteLine($"|{emergencyAppointment.Id,-15} |{emergencyAppointment.DoctorUsername,-17} |{emergencyAppointment.PatientUsername,-18} |{emergencyAppointment.AppointmentType,-17} |{emergencyAppointment.TimeSlot.StartTime.ToString("dd.MM.yyyy HH:mm"),-18} |{emergencyAppointment.TimeSlot.EndTime.ToString("dd.MM.yyyy HH:mm"),-19} |");
            Console.WriteLine("==============================================================================================================");
        }

    }
}
