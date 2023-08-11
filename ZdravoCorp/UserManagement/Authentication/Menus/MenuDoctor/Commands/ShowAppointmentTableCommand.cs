using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.View;
using ZdravoCorp.SchedulingManagement.Appointments.DoctorAppointment.ViewModel;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuDoctor.Commands
{
    public class ShowAppointmentTableCommand : BaseCommand
    {
        private Doctor _loggedDoctor;
        public ShowAppointmentTableCommand(Doctor loggedDoctor)
        {
            _loggedDoctor = loggedDoctor;
        }
        public override void Execute(object? parameter)
        {
            var view = new DoctorAppointmentsView();
            view.DataContext = new DoctorAppointmentsViewModel(view, _loggedDoctor);
            view.ShowDialog();
        }

    }
}
