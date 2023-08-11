using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZdravoCorp.CommunicatonManagement.Notifications.Service;
using ZdravoCorp.CommunicatonManagement.Polls.Model;
using ZdravoCorp.CommunicatonManagement.Polls.Repository;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Authentication.Login.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands;
using ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.View;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.ViewModel
{
    public class MenuPatientViewModel : BaseViewModel
    {
        public PatientService PatientService { get; set; }
        public Patient LoggedPatient { get; set; }
        public MenuPatientView MenuPatientView { get; }
        public PatientNotificationService PatientNotificationService { get; set; }
        public DoctorService DoctorService { get; set; }
        public AppointmentService AppointmentService { get; set; }
        public PatientAppointmentActionsService PatientAppointmentActionsService { get; set; }
        public PollService PollService { get; set; }

        public ICommand LogoutCommand { get; }
        public ICommand ShowPatientAppointmentsCommand { get; }
        public ICommand ShowPatientMedicalRecordCommand { get; }
        public ICommand ShowPatientSearchDoctorCommand { get; }
        public ICommand ShowPatientNotificationCommand { get; }
        public ICommand ShowHospitalPollCommand { get; }

        public MenuPatientViewModel(MenuPatientView menuPatientView, PatientService patientService, Patient loggedPatient)
        {
            MenuPatientView = menuPatientView;
            PatientService = patientService;
            LoggedPatient = loggedPatient;
            DoctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            AppointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            PatientNotificationService = new PatientNotificationService();
            PatientAppointmentActionsService = new PatientAppointmentActionsService(LoggedPatient, new PatientAppointmentActionsRepository(new Serializer<PatientAppointmentActions>()));
            PollService = new PollService(new PollRepository(new Serializer<Poll>()));
            LogoutCommand = new LogoutCommand(MenuPatientView, PatientNotificationService);
            ShowPatientAppointmentsCommand = new ShowPatientAppointmentsCommand(null, LoggedPatient, DoctorService, AppointmentService, PatientService, PatientAppointmentActionsService);
            ShowPatientMedicalRecordCommand = new ShowPatientMedicalRecordCommand(LoggedPatient, DoctorService, AppointmentService, PatientService, PollService);
            ShowPatientSearchDoctorCommand = new ShowPatientSearchDoctorCommand(LoggedPatient, DoctorService, AppointmentService, PatientService);
            PatientNotificationService.StartNotificationTimer();
            ShowPatientNotificationCommand = new ShowPatientNotificationCommand(PatientNotificationService, LoggedPatient);
            ShowHospitalPollCommand = new ShowHospitalPollCommand(LoggedPatient, PollService);
        }
    }
}
