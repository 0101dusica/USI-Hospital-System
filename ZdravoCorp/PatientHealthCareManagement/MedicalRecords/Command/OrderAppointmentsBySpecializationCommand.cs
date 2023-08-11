using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class OrderAppointmentsBySpecializationCommand : BaseCommand
    {

        public PatientMedicalRecordViewModel PatientMedicalRecordViewModel { get; set; }
        public AppointmentService AppointmentService { get; set; }
        public DoctorService DoctorService { get; set; }

        public OrderAppointmentsBySpecializationCommand(PatientMedicalRecordViewModel patientMedicalRecordViewModel, AppointmentService appointmentService, DoctorService doctorService)
        {
            PatientMedicalRecordViewModel = patientMedicalRecordViewModel;
            AppointmentService = appointmentService;
            DoctorService = doctorService;
        }

        public override void Execute(object? parameter)
        {
            PatientMedicalRecordViewModel.Appointments = new ObservableCollection<Appointment>(AppointmentService.OrderBySpecialization(PatientMedicalRecordViewModel.Appointments!, DoctorService));
        }



    }
}
