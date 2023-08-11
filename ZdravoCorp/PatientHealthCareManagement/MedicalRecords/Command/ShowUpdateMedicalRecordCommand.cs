using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Service;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.View;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class ShowUpdateMedicalRecordCommand<T> : BaseCommand where T : BaseViewModel
    {
        private PatientDoctorAssignmentService _service;

        private T _viewModel;

        private readonly Action<bool> _resultCallback;
        public ShowUpdateMedicalRecordCommand(T viewModel, Action<bool> resultCallback)
        {
            _viewModel = viewModel;
            _service = new PatientDoctorAssignmentService();
            _resultCallback = resultCallback;
        }

        private bool CanExecute(object parameter)
        {
            switch (_viewModel)
            {
                case DoctorPatientListViewModel doctorPatientListViewModel:
                    return doctorPatientListViewModel.SelectedPatient != null && _service.IsPatientAssignedToDoctor(doctorPatientListViewModel.LoggedDoctor.Username, doctorPatientListViewModel.SelectedPatient.Username);

                case DoctorExaminationViewModel doctorExaminationViewModel:
                    return doctorExaminationViewModel.Appointment.PatientUsername != null;

                default:
                    return false;
            }

        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                var view = new UpdateMedicalRecordView();
                switch (_viewModel)
                {
                    case DoctorPatientListViewModel doctorPatientListViewModel when doctorPatientListViewModel.SelectedPatient != null:
                        view.DataContext = new UpdateMedicalRecordViewModel(doctorPatientListViewModel.SelectedPatient.Username, doctorPatientListViewModel);
                        break;

                    case DoctorExaminationViewModel doctorExaminationViewModel when doctorExaminationViewModel.Appointment?.PatientUsername != null:
                        view.DataContext = new UpdateMedicalRecordViewModel(doctorExaminationViewModel.Appointment.PatientUsername);
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
