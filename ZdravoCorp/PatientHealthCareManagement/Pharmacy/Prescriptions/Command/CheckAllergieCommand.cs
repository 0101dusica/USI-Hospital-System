using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Model;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Repository;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.Service;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Command
{
    public class CheckAllergieCommand : BaseCommand
    {
        private AppointmentService _appointmentService;
        private PatientService _patientService;
        private MedicineService _medicineService;


        private AddPrescriptionViewModel _viewModel;

        public event EventHandler<bool> AllergiesChecked;

        public CheckAllergieCommand(AddPrescriptionViewModel viewModel)
        {
            _viewModel = viewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _medicineService = new MedicineService(new MedicineRepository(new Serializer<Medicine>()));

        }

        private bool CanExecute(object parameter)
        {

            return _viewModel.SelectedMedicine != null;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Patient patient = _patientService.GetByUsername(_viewModel.Appointment.PatientUsername);
                List<string> allergies = patient.MedicalRecord.Allergies;
                if (_medicineService.IsPatientAllergic(_viewModel.SelectedMedicine, allergies))
                {
                    AllergiesChecked?.Invoke(this, true);
                }
                else
                {
                    AllergiesChecked?.Invoke(this, false);
                }
            }

        }
    }
}
