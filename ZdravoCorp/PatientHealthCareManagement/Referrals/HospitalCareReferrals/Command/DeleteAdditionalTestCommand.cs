using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Command
{
    public class DeleteAdditionalTestCommand : BaseCommand
    {
        private AddHospitalReferralViewModel _viewModel;
        public DeleteAdditionalTestCommand(AddHospitalReferralViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.SelectedAdditionalTest);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.AdditionalTests.Remove(_viewModel.SelectedAdditionalTest);
            }
        }
    }
}
