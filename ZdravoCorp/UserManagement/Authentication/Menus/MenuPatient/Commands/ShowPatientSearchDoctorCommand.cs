using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Doctors.View;
using ZdravoCorp.UserManagement.Doctors.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands
{
    public class ShowPatientSearchDoctorCommand : BaseCommand
    {

        public Patient LoggedPatient { get; set; }
        public DoctorService DoctorService { get; set; }
        public PatientService PatientService { get; set; }
        public AppointmentService AppointmentService { get; set; }

        public ShowPatientSearchDoctorCommand(Patient loggedPatient, DoctorService doctorService,
            AppointmentService appointmentService, PatientService patientService)
        {
            LoggedPatient = loggedPatient;
            DoctorService = doctorService;
            PatientService = patientService;
            AppointmentService = appointmentService;

        }
        public override void Execute(object? parameter)
        {
            PatientSearchDoctorsView patientSearchDoctorsView = new PatientSearchDoctorsView();
            patientSearchDoctorsView.DataContext = new PatientSearchDoctorsViewModel(LoggedPatient, AppointmentService, DoctorService, PatientService);
            patientSearchDoctorsView.ShowDialog();
        }
    }
}
