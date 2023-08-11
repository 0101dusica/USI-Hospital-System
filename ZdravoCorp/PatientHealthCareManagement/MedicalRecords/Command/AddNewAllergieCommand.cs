using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class AddNewAllergieCommand : BaseCommand
    {
        private UpdateMedicalRecordViewModel _viewModel;
        public AddNewAllergieCommand(UpdateMedicalRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.NewAllergie);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.Allergies.Add(_viewModel.NewAllergie);
                _viewModel.NewAllergie = string.Empty;

            }
        }
    }
}
