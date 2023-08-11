using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.CommunicatonManagement.Polls.Model;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.CommunicatonManagement.Polls.ViewModel;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.CommunicatonManagement.Polls.Command
{
    public class SubmitHospitalPollCommand : BaseCommand
    {

        private HospitalPollViewModel _patientPollAboutHospital;
        private Patient _patient;
        private PollService _pollService;

        public SubmitHospitalPollCommand(HospitalPollViewModel patientPollAboutHospital, Patient patient, PollService pollService)
        {
            _patientPollAboutHospital = patientPollAboutHospital;
            _patient = patient;
            _pollService = pollService;

        }
        public override void Execute(object? parameter)
        {
            if (_patientPollAboutHospital.Validate())
            {
                List<string> questions = _patientPollAboutHospital.GetQuestions();
                string comment = _patientPollAboutHospital.Comment!;
                List<int> grades = _patientPollAboutHospital.GetGrades();
                Poll hospitalPoll = new Poll(_pollService.NextId(), _patient.Username, PollType.Hospital, questions, comment, grades, null);
                _pollService.Add(hospitalPoll);
                MessageBox.Show("Successfully hospital poll made!");
                _patientPollAboutHospital.SetAllAnswersToNull();
                _patientPollAboutHospital.SetCommentTuNull();
            }
        }
        
    }
}

