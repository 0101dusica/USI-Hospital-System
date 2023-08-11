using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Service;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class ShowMedicalRecordCommand<T> : BaseCommand where T : BaseViewModel
    {
        private PatientDoctorAssignmentService _service;

        private T _viewModel;

        private readonly Action<bool> _resultCallback;
        public ShowMedicalRecordCommand(T viewModel, Action<bool> resultCallback)
        {
            _viewModel = viewModel;
            _service = new PatientDoctorAssignmentService(); //pormeniti
            _resultCallback = resultCallback;
        }

        private bool CanExecute(object parameter)
        {
            switch (_viewModel)
            {
                case DoctorPatientListViewModel doctorPatientListViewModel:
                    return doctorPatientListViewModel.SelectedPatient != null && _service.IsPatientAssignedToDoctor(doctorPatientListViewModel.LoggedDoctor.Username, doctorPatientListViewModel.SelectedPatient.Username);

                case DoctorAppointmentsViewModel doctorAppointmentsViewModel:
                    return doctorAppointmentsViewModel.SelectedAppointment?.PatientUsername != null;

                case DoctorExaminationViewModel doctorExaminationViewModel:
                    return doctorExaminationViewModel.Appointment.PatientUsername != null;

                case NursePatientsListViewModel nursePatientsListViewModel:
                    return nursePatientsListViewModel.SelectedPatient != null;

                default:
                    return false;
            }
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                var view = new MedicalRecordView();
                switch (_viewModel)
                {
                    case DoctorPatientListViewModel doctorPatientListViewModel when doctorPatientListViewModel.SelectedPatient != null:
                        view.DataContext = new MedicalRecordViewModel(doctorPatientListViewModel.SelectedPatient.Username);
                        break;

                    case DoctorAppointmentsViewModel doctorAppointmentsViewModel when doctorAppointmentsViewModel.SelectedAppointment?.PatientUsername != null:
                        view.DataContext = new MedicalRecordViewModel(doctorAppointmentsViewModel.SelectedAppointment.PatientUsername);
                        break;

                    case DoctorExaminationViewModel doctorExaminationViewModel when doctorExaminationViewModel.Appointment?.PatientUsername != null:
                        view.DataContext = new MedicalRecordViewModel(doctorExaminationViewModel.Appointment.PatientUsername);
                        break;
                    case NursePatientsListViewModel nursePatientsListViewModel when nursePatientsListViewModel.SelectedPatient != null:
                        view.DataContext = new MedicalRecordViewModel(nursePatientsListViewModel.SelectedPatient.Username);
                        break;

                    default:
                        _resultCallback?.Invoke(false);
                        return;
                }
                view.ShowDialog();
                _resultCallback?.Invoke(true);
            }
            else
            {
                _resultCallback?.Invoke(false);
            }
        }
    }
}
