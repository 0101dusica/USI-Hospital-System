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
    public class InsertPatientCommand : BaseCommand
    {
        private NursePatientsListViewModel _nursePatientsListViewModel { get; }

        public InsertPatientCommand(NursePatientsListViewModel nursePatientsListViewModel)
        {
            _nursePatientsListViewModel = nursePatientsListViewModel;
        }

        public override void Execute(object? parameter)
        {
            InsertPatientView insertPatientView = new InsertPatientView();
            insertPatientView.DataContext = new InsertPatientViewModel(insertPatientView, _nursePatientsListViewModel);
            insertPatientView.ShowDialog();
        }
    }
}
