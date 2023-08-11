using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Service;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.ViewModel;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.View;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Command
{
    public class VisitPatientCommand : BaseCommand
    {
        private HospitalCareTableViewModel _viewModel;
        private HospitalCareService _service;

        public event EventHandler<bool> VisitStarted;

        public VisitPatientCommand(HospitalCareTableViewModel viewModel)
        {
            _viewModel = viewModel;
            _service = new HospitalCareService(new HospitalCareRepository(new Serializer<HospitalCare>()));
        }

        private bool CanExecute(object parameter)
        {
            return _viewModel.SelectedHospitalCare != null && _viewModel.SelectedHospitalCare.TimeSlot.StartTime.Date <= DateTime.Today.Date && DateTime.Today.Date <= _viewModel.SelectedHospitalCare.TimeSlot.EndTime.Date && _viewModel.SelectedHospitalCare.HospitalCareStatus == HospitalCareStatus.InProgress;
        }

        public override void Execute(object? parameter)
        {
            if (CanExecute(parameter))
            {
                Window view = new HospitalCarePatientView();
                view.DataContext = new HospitalCarePatientViewModel(view,_viewModel.SelectedHospitalCare, _viewModel);
                view.ShowDialog();
                VisitStarted?.Invoke(this, true);


            }
            else { VisitStarted?.Invoke(this, false); }
        }
    }
}
