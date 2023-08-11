using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Repository;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Service;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.ViewModel;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Command
{
    public class AddSpecialistReferralCommand : BaseCommand
    {
        private SpecialistReferralService _specialistReferralService;

        private AddSpecialistReferralViewModel _viewModel;

        public event EventHandler<bool> SpecialistReferralAdded;

        public AddSpecialistReferralCommand(AddSpecialistReferralViewModel viewModel)
        {
            _viewModel = viewModel;
            _specialistReferralService = new SpecialistReferralService(new SpecialistReferralRepository(new Serializer<SpecialistReferral>()));
        }

        private bool CanExecute(object parameter)
        {
            if (_viewModel.SelectedDoctor != null)
            {
                return true;
            }
            else
            {
                return _specialistReferralService.IsDoctorWithSpecializationExist(_viewModel.LoggedDoctor, _viewModel.SelectedSpecialization);
            }

        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                if (_viewModel.SelectedDoctor == null)
                {
                    int randomIndex = new Random().Next(_viewModel.DoctorsTable.Count);
                    Doctor randomDoctor = _viewModel.DoctorsTable[randomIndex];

                    SpecialistReferral newRefferal = new SpecialistReferral(_specialistReferralService.NextId(),_viewModel.PatientUsername, _viewModel.LoggedDoctor.Username, randomDoctor.Username, SpecialistReferralStatus.Created);
                    _specialistReferralService.Add(newRefferal);
                }
                else
                {
                    SpecialistReferral newRefferal = new SpecialistReferral(_specialistReferralService.NextId(),_viewModel.PatientUsername, _viewModel.LoggedDoctor.Username, _viewModel.SelectedDoctor.Username, SpecialistReferralStatus.Created);
                    _specialistReferralService.Add(newRefferal);
                }

                SpecialistReferralAdded?.Invoke(this, true);
            }
            else { SpecialistReferralAdded?.Invoke(this, false); }
        }
    }
}
