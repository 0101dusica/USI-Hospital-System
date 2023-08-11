using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Patients.View;
using ZdravoCorp.UserManagement.Patients.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands
{
    public class PatientsCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            NursePatientsListView nursePatientsListView = new NursePatientsListView();
            nursePatientsListView.patientsDataGrid.Items.Clear();
            nursePatientsListView.DataContext = new NursePatientsListViewModel();
            nursePatientsListView.ShowDialog();
        }
    }
}
