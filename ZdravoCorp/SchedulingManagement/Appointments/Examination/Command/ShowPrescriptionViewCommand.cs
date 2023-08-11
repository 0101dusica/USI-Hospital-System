using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.View;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.SchedulingManagement.Appointments.Examination.Command
{
    public class ShowPrescriptionViewCommand<T> : BaseCommand where T : BaseViewModel
    {

        private T _viewModel;
        public ShowPrescriptionViewCommand(T viewModel)
        {
            _viewModel = viewModel;

        }
        public override void Execute(object? parameter)
        {
            var view = new AddPrescriptionView();
            switch (_viewModel)
            {
                case AddHospitalReferralViewModel addHospitalReferralViewModel:
                    view.DataContext = new AddPrescriptionViewModel(addHospitalReferralViewModel.Appointment);
                    break;

                case DoctorExaminationViewModel doctorExaminationViewModel:
                    view.DataContext = new AddPrescriptionViewModel(doctorExaminationViewModel.Appointment);
                    break;

                default:
                    return;
            }

            view.ShowDialog();
                
        }
    }
}

