using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Repository;
using ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Service;
using ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.ViewModel;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Patients.Model;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.AppointmentByReferral.Command
{
    public class GetPatientReferralCommand : BaseCommand
    {
        private NurseCreateAppointmentViewModel _viewModel;
        private SpecialistReferralService _specialistReferralService;
        private DoctorService _doctorService;

        public GetPatientReferralCommand(NurseCreateAppointmentViewModel nurseCreateAppointmentViewModel)
        {
            _specialistReferralService = new SpecialistReferralService(new SpecialistReferralRepository(new Serializer<SpecialistReferral>()));
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _viewModel = nurseCreateAppointmentViewModel;
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.isPatientSelected();
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Patient selectedPatient = _viewModel.SelectedPatient;
                _viewModel.ReferralTable.Clear();
                var referrals = _specialistReferralService.GetPatientsSpecialistRefferals(selectedPatient.Username);
                var generalPracticioners = _doctorService.GetGeneralPractitioners();

                var filteredSpecialistReferralList = referrals
                .Where(referral => generalPracticioners
                .Any(generalPractitioner => referral.FromDoctor.Equals(generalPractitioner.Username)))
                .ToList();

                if (filteredSpecialistReferralList.Count == 0)
                {
                    MessageBox.Show($"The patient does not have referrals issued by a general practitioner!");
                }
                else
                {
                    foreach (var referral in filteredSpecialistReferralList)
                    {
                        _viewModel.ReferralTable.Add(referral);
                    }
                }
            }
        }
    }
}
