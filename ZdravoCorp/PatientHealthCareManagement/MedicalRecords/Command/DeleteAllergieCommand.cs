using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class DeleteAllergieCommand : BaseCommand
    {
        private UpdateMedicalRecordViewModel _viewModel;
        public DeleteAllergieCommand(UpdateMedicalRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.SelectedAllergie);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.Allergies.Remove(_viewModel.SelectedAllergie);
            }
        }
    }
}
