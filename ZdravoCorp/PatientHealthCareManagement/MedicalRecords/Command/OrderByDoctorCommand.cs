using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class OrderByDoctorCommand : BaseCommand
    {
        public DoctorService DoctorService { get; set; }

        PatientMedicalRecordViewModel PatientMedicalRecordViewModel { get; set; }
        public OrderByDoctorCommand(PatientMedicalRecordViewModel patientMedicalRecordViewModel, DoctorService doctorService)
        {
            PatientMedicalRecordViewModel = patientMedicalRecordViewModel;

            DoctorService = doctorService;

        }
        public override void Execute(object? parameter)
        {
            PatientMedicalRecordViewModel.Appointments = new ObservableCollection<Appointment>(PatientMedicalRecordViewModel.AppointmentService.OrderByDoctor(PatientMedicalRecordViewModel.Appointments));

        }
    }
}
