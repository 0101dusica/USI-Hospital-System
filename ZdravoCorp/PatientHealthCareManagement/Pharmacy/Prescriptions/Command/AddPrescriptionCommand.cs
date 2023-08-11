using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.ViewModel;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Command
{
    public class AddPrescriptionCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private PrescriptionService _prescriptionService;

        private AddPrescriptionViewModel _viewModel;

        public event EventHandler<bool> PrescriptionAdded;

        public AddPrescriptionCommand(AddPrescriptionViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _prescriptionService = new PrescriptionService(new PrescriptionRepository(new Serializer<Prescription>()));

        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedMedicine != null && _viewModel.SelectedDate >= DateTime.Now.Date && _viewModel.DailyUsage > 0 && _viewModel.Duration > 0;
        }


        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                TimeSlot timeSlot = new TimeSlot(_viewModel.SelectedDate, _viewModel.SelectedDate.AddDays(_viewModel.Duration));
                Prescription newPrescription = new Prescription(_prescriptionService.NextId(), _viewModel.DailyUsage, _viewModel.SelectedInstruction, _viewModel.Time, _viewModel.Appointment.PatientUsername, _viewModel.Appointment.DoctorUsername, timeSlot, _viewModel.SelectedMedicine.Id);
                _prescriptionService.Add(newPrescription);

                PrescriptionAdded?.Invoke(this, true);
            }
            else { PrescriptionAdded?.Invoke(this, false); }
        }
    }
}
