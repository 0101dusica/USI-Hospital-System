using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.View;
using ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands
{
    public class ShowNurseAppointmentWindowCommand : BaseCommand
    {

        public override void Execute(object? parameter)
        {
            NurseCreateAppointmentView nurseCreateAppointmentView = new NurseCreateAppointmentView();
            nurseCreateAppointmentView.DataContext = new NurseCreateAppointmentViewModel();
            nurseCreateAppointmentView.ShowDialog();
        }
    }
}
