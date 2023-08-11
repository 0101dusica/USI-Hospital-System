using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Repository;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Service;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.ViewModel;
using ZdravoCorp.SchedulingManagement;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Command
{
    public class AddHospitalReferralCommand : BaseCommand
    {
        private HospitalCareReferralService _hospitalCareReferralService;

        private AddHospitalReferralViewModel _viewModel;

        public event EventHandler<bool> HospitalReferralAdded;


        public AddHospitalReferralCommand(AddHospitalReferralViewModel viewModel)
        {
            _viewModel = viewModel;
            _hospitalCareReferralService = new HospitalCareReferralService(new HospitalCareReferralRepository(new Serializer<HospitalCareReferral>()));
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.Days > 0 && _viewModel.Description != null && _viewModel.SelectedDate > DateTime.Now.Date;
        }


        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                TimeSlot timeSlot = new TimeSlot(_viewModel.SelectedDate, _viewModel.SelectedDate.AddDays(_viewModel.Days));
                HospitalCareReferral newReferral = new HospitalCareReferral(_hospitalCareReferralService.NextId(), _viewModel.PatientUsername,_viewModel.DoctorUsername, timeSlot, _viewModel.AdditionalTests.ToList(), _viewModel.Description);
                _hospitalCareReferralService.Add(newReferral);
                HospitalReferralAdded?.Invoke(this, true);
            }
            else
            {
                HospitalReferralAdded?.Invoke(this, false);
            }
        }
    }
}
