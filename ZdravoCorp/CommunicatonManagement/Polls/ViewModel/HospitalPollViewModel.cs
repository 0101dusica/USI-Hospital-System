using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZdravoCorp.CommunicatonManagement.Polls.Command;
using ZdravoCorp.CommunicatonManagement.Polls.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.ViewModel;

namespace ZdravoCorp.CommunicatonManagement.Polls.ViewModel
{
    public class HospitalPollViewModel : BaseViewModel
    {
        private string _question1;
        public string Question1
        {
            get => _question1;
            set
            {
                _question1 = value;
                OnPropertyChanged(nameof(Question1));
            }
        }

        private string _question2;
        public string Question2
        {
            get => _question2;
            set
            {
                _question2 = value;
                OnPropertyChanged(nameof(Question2));
            }
        }

        private string _question3;
        public string Question3
        {
            get => _question3;
            set
            {
                _question3 = value;
                OnPropertyChanged(nameof(Question3));
            }
        }

        private string _question4;
        public string Question4
        {
            get => _question4;
            set
            {
                _question4 = value;
                OnPropertyChanged(nameof(Question4));
            }
        }



        public ICommand SubmitHospitalPollCommand { get; set; }
        private string? _answer1;
        public string? Answer1
        {
            get => _answer1;
            set
            {
                _answer1 = value;
                OnPropertyChanged(nameof(Answer1));
            }
        }
        private string? _answer2;
        public string? Answer2
        {
            get => _answer2;
            set
            {
                _answer2 = value;
                OnPropertyChanged(nameof(Answer2));
            }
        }

        private string? _answer3;
        public string? Answer3
        {
            get => _answer3;
            set
            {
                _answer3 = value;
                OnPropertyChanged(nameof(Answer3));
            }
        }

        private string? _answer4;
        public string? Answer4
        {
            get => _answer4;
            set
            {
                _answer4 = value;
                OnPropertyChanged(nameof(Answer4));
            }
        }

        private string? _comment;
        public string? Comment
        {
            get => _comment;
            set
            {
                _comment = value;
                OnPropertyChanged(nameof(Comment));
            }
        }

       
        public HospitalPollViewModel(Patient patient, PollService pollService)
        {
            _question1 = "1. Hospital service quality, your opinion ";
            _question2 = "2. Cleanliness inside the hospital, your opinion";
            _question3 = "3. How satisfied are you as a patient with the work of the hospital";
            _question4 = "4. Would you recommend this hospital to friends";
            SubmitHospitalPollCommand = new SubmitHospitalPollCommand(this, patient, pollService);

        }

        public bool Validate()
        {
            if (!ValidateRadioButtonSelection())
            {
                MessageBox.Show("You must select answer for all questions!");
                return false;
            }
            if (!ValidateCommentInput())
            {
                MessageBox.Show("You must put some comment for this poll!");
                return false;
            }
            return true;
        }

        public bool ValidateRadioButtonSelection()
        {
            return !(Answer1 == null || Answer1.Equals("0") || Answer2 == null || Answer2.Equals("0")
                     || Answer3 == null || Answer3.Equals("0")
                     || Answer4 == null || Answer4.Equals("0"));
        }

        public bool ValidateCommentInput()
        {
            return !(Comment == null || Comment.Length < 10);
        }

        public void SetAllAnswersToNull()
        {
            Answer1 = null;
            Answer2 = null;
            Answer3 = null;
            Answer4 = null;

        }
        public void SetCommentTuNull()
        {
            Comment = null;
        }
        public List<string> GetQuestions()
        {
            return new List<string>
            {
                _question1,
                _question2,
                _question3,
                _question4
            };
        }

        public List<int> GetGrades()
        {
            return new List<int>
            {
                int.Parse(_answer1!),
                int.Parse(_answer2!),
                int.Parse(_answer3!),
                int.Parse(_answer4!)
            };
        }

    }
}
