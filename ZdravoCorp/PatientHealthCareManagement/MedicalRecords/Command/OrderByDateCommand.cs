using System.Collections.ObjectModel;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class OrderByDateCommand : BaseCommand

    {
        public AppointmentService AppointmentService { get; set; }
        public PatientMedicalRecordViewModel PatientMedicalRecordViewModel { get; set; }
        public OrderByDateCommand(PatientMedicalRecordViewModel patientMedicalRecordViewModel,
            AppointmentService appointmentService)
        {
            PatientMedicalRecordViewModel = patientMedicalRecordViewModel;
            AppointmentService = appointmentService;

        }
        public override void Execute(object? parameter)
        {
            PatientMedicalRecordViewModel.Appointments = new ObservableCollection<Appointment>(AppointmentService.OrderByDate(PatientMedicalRecordViewModel.Appointments));

        }
    }
}
