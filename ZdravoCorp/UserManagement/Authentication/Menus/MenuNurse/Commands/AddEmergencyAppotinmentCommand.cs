using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.View;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuNurse.Commands
{
    public class AddEmergencyAppotinmentCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            AddEmergencyAppointmentView addEmergencyAppointmentView = new AddEmergencyAppointmentView();
            addEmergencyAppointmentView.DataContext = new AddEmergencyAppointmentViewModel(addEmergencyAppointmentView);
            addEmergencyAppointmentView.ShowDialog();
        }
    }
}
