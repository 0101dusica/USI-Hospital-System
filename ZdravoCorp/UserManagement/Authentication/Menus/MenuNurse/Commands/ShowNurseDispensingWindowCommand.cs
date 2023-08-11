using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.View;
using ZdravoCorp.PatientHealthCareManagement.Pharmacy.Medicines.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands
{
    public class ShowNurseDispensingWindowCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            NurseMedicationDispenserView nurseMedicationDispenserView = new NurseMedicationDispenserView();
            nurseMedicationDispenserView.DataContext = new NurseMedicationDispenserViewModel();
            nurseMedicationDispenserView.ShowDialog();
        }
    }
}
