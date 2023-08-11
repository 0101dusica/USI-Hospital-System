using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.Examination.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.SchedulingManagement.Appointments.Examination.Command
{
    public class AddSypmtomCommand : BaseCommand
    {
        private DoctorExaminationViewModel _viewModel;
        public AddSypmtomCommand(DoctorExaminationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        private bool CanExecute(object parameter)
        {
            return !string.IsNullOrEmpty(_viewModel.NewSymptom);
        }
        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                _viewModel.Symptoms.Add(_viewModel.NewSymptom);
                _viewModel.NewSymptom = string.Empty;

            }
        }
    }
}
