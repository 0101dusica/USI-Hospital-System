using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Service;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.UserManagement.Patients.Command
{
    public class OpenPatientAdmissionWindowCommand : BaseCommand
    {
        private NursePatientsListViewModel _nursePatientsListViewModel;
        private AppointmentService _appointmentService;

        public OpenPatientAdmissionWindowCommand(NursePatientsListViewModel nursePatientsListViewModel)
        {
            _nursePatientsListViewModel = nursePatientsListViewModel;
            _appointmentService = new AppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
        }

        public override void Execute(object? parameter)
        {

            if (_nursePatientsListViewModel.isPatientSelected())
            {
                Appointment? patientEarliestAppointment = _appointmentService.GetEarliestPatientAppointment(_nursePatientsListViewModel.SelectedPatient.Username);
                if (patientEarliestAppointment != null)
                {
                    PatientAdmissionView patientAdmissionView = new PatientAdmissionView();
                    patientAdmissionView.DataContext = new PatientAdmissionViewModel(_nursePatientsListViewModel.SelectedPatient, patientEarliestAppointment, patientAdmissionView, _nursePatientsListViewModel);
                    patientAdmissionView.ShowDialog();
                }
                else
                {
                    MessageBox.Show($"The patient does not have any appointments for the next 15 minutes!");
                }
            }
        }

    }
}
