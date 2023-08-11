using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Patients.Command
{
    public class EditPatientCommand : BaseCommand
    {
        private NursePatientsListViewModel _nursePatientsListViewModel;

        public EditPatientCommand(NursePatientsListViewModel nursePatientsListViewModel)
        {
            _nursePatientsListViewModel = nursePatientsListViewModel;
        }

        public override void Execute(object? parameter)
        {
            Console.WriteLine(_nursePatientsListViewModel.SelectedPatient.Username);
            EditPatientView editPatientView = new EditPatientView();
            editPatientView.DataContext = new EditPatientViewModel(_nursePatientsListViewModel.SelectedPatient, editPatientView, _nursePatientsListViewModel);
            editPatientView.ShowDialog();
        }
    }
}
