using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands

{
    public class ShowPatientMedicalRecordCommand : BaseCommand
    {
        public Patient LoggedPatient { get; set; }
        public DoctorService DoctorService { get; set; }
        public PatientService PatientService { get; set; }
        public AppointmentService AppointmentService { get; set; }

        public PollService PollService { get; set; }

        public ShowPatientMedicalRecordCommand(Patient loggedPatient, DoctorService doctorService,
            AppointmentService appointmentService, PatientService patientService, PollService pollService)
        {
            LoggedPatient = loggedPatient;
            DoctorService = doctorService;
            PatientService = patientService;
            AppointmentService = appointmentService;
            PollService = pollService;

        }
        public override void Execute(object? parameter)
        {
            PatientMedicalRecordView patientMedicalRecordView = new PatientMedicalRecordView();
            patientMedicalRecordView.DataContext = new PatientMedicalRecordViewModel(LoggedPatient, DoctorService, AppointmentService, PatientService, PollService);
            patientMedicalRecordView.ShowDialog();
        }
    }
}
