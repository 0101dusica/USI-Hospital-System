using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.UserManagement.Patients.Repository;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Patients.Command
{
    public class AddPatientAdmissionCommand : BaseCommand
    {
        private NursePatientsListViewModel _nursePatientsListViewModel;
        private Patient _patient;
        private Appointment _appointment;
        private PatientAdmissionViewModel _patientAdmissionViewModel;
        private PatientAdmissionView _patientAdmissionView;
        private PatientService _patientService;
        private AppointmentService _appointmentService;

        public AddPatientAdmissionCommand(NursePatientsListViewModel nursePatientsListViewModel, PatientAdmissionViewModel patientAdmissionViewModel, PatientAdmissionView patientAdmissionView, Patient patient, Appointment appointment)
        {
            _nursePatientsListViewModel = nursePatientsListViewModel;
            _patientService = new PatientService(new PatientRepository(new Serializer<Patient>()));
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _patientAdmissionViewModel = patientAdmissionViewModel;
            _patientAdmissionView = patientAdmissionView;
            _patient = patient;
            _appointment = appointment;
        }

        public override void Execute(object? parameter)
        {
            List<string> newAllergies = _patientAdmissionViewModel.Allergies.ToList();
            List<string> newMedicalHistory = _patientAdmissionViewModel.MedicalHistory.ToList();
            List<string> newSymptoms = _patientAdmissionViewModel.Symptoms.ToList();

            _patientService.UpdateMedicalRecordAllergies(_patient.Username, newAllergies);
            _patientService.UpdateMedicalRecordMedicalHistory(_patient.Username, newMedicalHistory);
            _appointmentService.UpdateAnamnesisSymptoms(_appointment.Id, newSymptoms);

            MessageBox.Show($"Successful patient admission!" + _patient.FirstName + " " + _patient.LastName);

            _patientAdmissionView.Close();
            _nursePatientsListViewModel.PatientsTable = _nursePatientsListViewModel.LoadPatients();

        }
    }
}
