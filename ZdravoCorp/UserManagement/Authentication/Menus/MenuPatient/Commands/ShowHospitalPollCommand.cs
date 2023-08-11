using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.CommunicatonManagement.Polls.View;
using ZdravoCorp.CommunicatonManagement.Polls.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Authentication.Menus.MenuPatient.Commands
{
    public class ShowHospitalPollCommand : BaseCommand
    {
        public Patient Patient { get; set; }
        private PollService _pollService;

        public ShowHospitalPollCommand(Patient loggedPatient, PollService pollService)
        {
            Patient = loggedPatient;
            _pollService = pollService;

        }

        public override void Execute(object? parameter)
        {
            HospitalPollView pollHospitalView = new HospitalPollView();
            pollHospitalView.DataContext = new HospitalPollViewModel(Patient, _pollService);
            pollHospitalView.ShowDialog();
        }
    }
}
