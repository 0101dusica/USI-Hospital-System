using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class DeleteMedicalHistoryCommand : BaseCommand
    {
        private UpdateMedicalRecordViewModel _viewModel;
        public DeleteMedicalHistoryCommand(UpdateMedicalRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.SelectedMedicalHistory);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.MedicalHistory.Remove(_viewModel.SelectedMedicalHistory);
            }
        }
    }
}
