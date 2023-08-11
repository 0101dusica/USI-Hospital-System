using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model;
using ZdravoCorp.Utils.ViewModel;
using ZdravoCorp.Utils.Serializer;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Service;
using ZdravoCorp.PatientHealthCareManagement.HospitalCares.Repository;
using ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Command;
using System.Windows.Input;
using System.Windows;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.SchedulingManagement.Appointments.Control.View;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Control.ViewModel;

namespace ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.ViewModel
{
    public class HospitalCarePatientViewModel : BaseViewModel
    {
        private ObservableCollection<string> _additionalTests = new ObservableCollection<string>();
        public ObservableCollection<string> AdditionalTests
        {
            get { return _additionalTests; }
            set { _additionalTests = value; OnPropertyChanged(nameof(AdditionalTests)); }
        }

        private string _therapy;
        public string Therapy
        {
            get { return _therapy; }
            set
            {
                _therapy = value;
                OnPropertyChanged(nameof(Therapy));
            }
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
            }
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        private bool _isDischargePatientChecked;
        public bool IsDischargePatientChecked
        {
            get { return _isDischargePatientChecked; }
            set
            {
                _isDischargePatientChecked = value;
                OnPropertyChanged(nameof(IsDischargePatientChecked));
                OnPropertyChanged(nameof(IsControlEnabled));
                UpdateDatePickerEnabled();
            }
        }

        private bool _isControlChecked;
        public bool IsControlChecked
        {
            get { return _isControlChecked; }
            set
            {
                if (_isDischargePatientChecked)
                {
                    _isControlChecked = value;
                }
                else
                {
                    _isControlChecked = false;
                }

                OnPropertyChanged(nameof(IsControlChecked));
            }
        }

        public bool IsControlEnabled
        {
            get { return IsDischargePatientChecked; }
        }

        private bool _isDatePickerEnabled = true;
        public bool IsDatePickerEnabled
        {
            get { return _isDatePickerEnabled; }
            set
            {
                _isDatePickerEnabled = value;
                OnPropertyChanged(nameof(IsDatePickerEnabled));
            }
        }

        public ICommand FinishVisitCommand { get; }
        
        private HospitalCareTableViewModel _viewModel;
        public ICommand ShowAddControlViewCommand { get; }
        private Window _view;
        public Window View
        {
            get { return _view; }
            set
            {
                _view = value;
                OnPropertyChanged(nameof(View));
            }
        }

        public HospitalCare SelectedHospitalCare { get; set; }

        private Doctor _loggedDoctor;
        public Doctor LoggedDoctor
        {
            get { return _loggedDoctor; }
        }

        public HospitalCarePatientViewModel(Window view,HospitalCare selectedHospitalCare, HospitalCareTableViewModel viewModel)
        {
            HospitalCareService service = new HospitalCareService(new HospitalCareRepository(new Serializer<HospitalCare>()));
            SelectedHospitalCare = selectedHospitalCare;
            _view = view;
            _viewModel = viewModel;
            _loggedDoctor = viewModel.LoggedDoctor;

            Therapy = selectedHospitalCare.Therapy;
            AdditionalTests = new ObservableCollection<string>(service.GetHospitalCareReferral(selectedHospitalCare.ReferralId).AdditionalTests);
            StartDate = selectedHospitalCare.TimeSlot.StartTime;
            EndDate = selectedHospitalCare.TimeSlot.EndTime;

            FinishVisitCommand = new FinishVisitCommand(this);
            ((FinishVisitCommand)FinishVisitCommand).VisitFinished += OnVisitFinished;

            ShowAddControlViewCommand = new RelayCommand(ShowAddControlView);

        }

        private void ShowAddControlView()
        {
            var view = new AddControlView();
            view.DataContext = new AddControlViewModel(view,_loggedDoctor,SelectedHospitalCare);
            view.ShowDialog();
        }

        private void OnVisitFinished(object? sender, bool success)
        {
            if (success)
            {
                _viewModel.HospitalCares = _viewModel.LoadHospitalCares();
            }
            else
            {
                MessageBox.Show("End date of hospital care is passed");

            }
        }

        private void UpdateDatePickerEnabled()
        {
            if (IsDischargePatientChecked)
            {
                EndDate = DateTime.Now.Date;
                IsDatePickerEnabled = false;
            }
            else
            {
                IsDatePickerEnabled = true;
            }
        }
    }
}
