using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Command
{
    public class AddNewMedicalHistoryCommand : BaseCommand
    {
        private UpdateMedicalRecordViewModel _viewModel;
        public AddNewMedicalHistoryCommand(UpdateMedicalRecordViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.NewMedicalHistory);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.MedicalHistory.Add(_viewModel.NewMedicalHistory);
                _viewModel.NewMedicalHistory = string.Empty;

            }
        }
    }
}
