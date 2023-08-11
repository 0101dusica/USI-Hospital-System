// See https://aka.ms/new-console-template for more information
using ZdravoCorp.UserManagement.Administrators.Model;
using ZdravoCorp.UserManagement.Administrators.Repository;
using ZdravoCorp.UserManagement.Administrators.Service;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Nurses.Model;
using ZdravoCorp.UserManagement.Nurses.Repository;
using ZdravoCorp.UserManagement.Nurses.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorpCLI.Views;

Program.Main();

partial class Program
{

    static void Main()
    {
        AdministratorService administratorService = new AdministratorService(new AdministratorRepository(new Serializer<Administrator>()));
        DoctorService doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
        NurseService nurseService = new NurseService(new NurseRepository(new Serializer<Nurse>()));
        PatientService patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));

        string username, password;
        bool isAuthenticated = false;

        do
        {
            Console.Write("Username: ");
            username = Console.ReadLine();

            Console.Write("Password: ");
            password = Console.ReadLine();

            var administrator = administratorService.GetByUsername(username);
            var doctor = doctorService.GetByUsername(username);
            var nurse = nurseService.GetByUsername(username);
            var patient = patientService.GetByUsername(username);

            switch (true)
            {
                case var _ when administrator != null && administrator.Password == password:
                    new ComplexRenovationsView();
                    isAuthenticated = true;
                    break;
                case var _ when doctor != null && doctor.Password == password:
                    new DoctorAppointmentView(doctor);
                    isAuthenticated = true;
                    break;
                case var _ when nurse != null && nurse.Password == password:
                    new EmergencyAppointmentView();
                    isAuthenticated = true;
                    break;
                case var _ when patient != null && patient.Password == password:
                    ShowPatientView(patient);
                    isAuthenticated = true;
                    break;
                default:
                    Console.WriteLine("Invalid username or password.");
                    isAuthenticated = false;
                    break;
            }

            Console.WriteLine();
        } while (!isAuthenticated);
    }



    private static void ShowAdministratorView(Administrator administrator)
    {
        Console.WriteLine("Administrator view");
    }

    private static void ShowDoctorView(Doctor doctor)
    {
        Console.WriteLine("Doctor view");
    }

    private static void ShowNurseView(Nurse nurse)
    {
        Console.WriteLine("Nurse view");
    }

    private static void ShowPatientView(Patient patient)
    {
        Console.WriteLine("Patient view");
    }
}

