using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.SchedulingManagement.Appointments.Examination.Command
{
    public class DeleteSypmtomCommand : BaseCommand
    {
        private DoctorExaminationViewModel _viewModel;
        public DeleteSypmtomCommand(DoctorExaminationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.SelectedSymptom);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.Symptoms.Remove(_viewModel.SelectedSymptom);
            }
        }
    }
}
