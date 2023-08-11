using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Service;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Service;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.ViewModel;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.UserManagement.Patients.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Command
{
    public class FinishVisitCommand : BaseCommand
    {
        private HospitalCarePatientViewModel _viewModel;

        private HospitalCareService _hospitalCareService;

        public event EventHandler<bool> VisitFinished;

        public FinishVisitCommand(HospitalCarePatientViewModel hospitalCarePatientViewModel)
        {
            _viewModel = hospitalCarePatientViewModel;
            _hospitalCareService = new HospitalCareService(new HospitalCareRepository(new Serializer<HospitalCare>()));
        }

        private bool CanExecute(object parameter)
        {
            if (!_viewModel.IsDischargePatientChecked)
            {
                return _viewModel.EndDate >= DateTime.Now.Date;

            }
            return true;
        }


        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                var selectedHospitalCare = _viewModel.SelectedHospitalCare;

                HospitalCare editedHospitalCare = new HospitalCare();
                editedHospitalCare.Id = selectedHospitalCare.Id;
                editedHospitalCare.Therapy = selectedHospitalCare.Therapy;
                editedHospitalCare.VisitIds = selectedHospitalCare.VisitIds;
                editedHospitalCare.ReferralId = selectedHospitalCare.ReferralId;
                editedHospitalCare.PatientUsername = selectedHospitalCare.PatientUsername;
                editedHospitalCare.RoomId = selectedHospitalCare.RoomId;

                if (_viewModel.IsDischargePatientChecked)
                {
                    editedHospitalCare.HospitalCareStatus = HospitalCareStatus.Finished;
                }
                else
                {
                    editedHospitalCare.HospitalCareStatus = selectedHospitalCare.HospitalCareStatus;

                }

                editedHospitalCare.Therapy = _viewModel.Therapy;
                editedHospitalCare.TimeSlot = new TimeSlot(_viewModel.StartDate, _viewModel.EndDate);

                _hospitalCareService.Update(editedHospitalCare);
                VisitFinished?.Invoke(this, true);

                _viewModel.View.Close();
            }
            else
            {
                VisitFinished?.Invoke(this, false);
            }
        }
    }
}
