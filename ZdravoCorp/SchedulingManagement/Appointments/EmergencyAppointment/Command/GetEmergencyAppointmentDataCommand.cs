using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Service;
using ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.ViewModel;
using ZdravoCorp.SchedulingManagement.Appointments.Model;
using ZdravoCorp.SchedulingManagement.Appointments.Repository;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Repository;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.Utils.Command;
using ZdravoCorp.Utils.Serializer;

namespace ZdravoCorp.SchedulingManagement.Appointments.EmergencyAppointment.Command
{
    public class GetEmergencyAppointmentDataCommand : BaseCommand
    {
        private DoctorService _doctorService;
        private EmergencyAppointmentService _emergencyappointmentService;
        private AddEmergencyAppointmentViewModel _addEmergencyAppointmentViewModel;
        private Dictionary<Doctor, List<Appointment>> _appointmentsForEachDoctor { get; set; }
        private Dictionary<Doctor, DateTime> _earliestTermForEachDoctor { get; set; }
        private Doctor? _choosenDoctor { get; set; }
        private DateTime _currentTime;
        private DateTime _twoHoursFromNow;


        public GetEmergencyAppointmentDataCommand(AddEmergencyAppointmentViewModel addEmergencyAppointmentViewModel, DateTime currentTime, DateTime twoHoursFromNow) { 
            _addEmergencyAppointmentViewModel = addEmergencyAppointmentViewModel;
            _doctorService = new DoctorService(new DoctorRepository(new Serializer<Doctor>()));
            _emergencyappointmentService = new EmergencyAppointmentService(new AppointmentRepository(new Serializer<Appointment>()));
            _appointmentsForEachDoctor = new Dictionary<Doctor, List<Appointment>>();
            _earliestTermForEachDoctor = new Dictionary<Doctor, DateTime>();
            _currentTime = currentTime;
            _twoHoursFromNow = twoHoursFromNow;
        }

        public override void Execute(object? parameter)
        {
            _currentTime = _currentTime.AddSeconds(-_currentTime.Second);
            _addEmergencyAppointmentViewModel.CheckEmergencyDuration();
            bool isDurationTimeValid = true;
            if (_addEmergencyAppointmentViewModel.IsSurgerySelected)
            {
                isDurationTimeValid = _addEmergencyAppointmentViewModel.ValidateSurgeryDurationInput(_addEmergencyAppointmentViewModel.SurgeryDuration);
                if (!isDurationTimeValid)
                {
                    MessageBox.Show($"Check again! Surgery duration is not valid!");
                }

            } 
            if(isDurationTimeValid || _addEmergencyAppointmentViewModel.IsAppointmentSelected)
            {

                
                List<Doctor> specializedDoctors = _doctorService.FilterBySpecialization(_addEmergencyAppointmentViewModel.SelectedSpecialization);
                List<Doctor> doctorsWithoutAppointments = _doctorService.GetDoctorWithoutAppointments();
                if (doctorsWithoutAppointments.Count != 0)
                {
                    _choosenDoctor = doctorsWithoutAppointments[0];
                    _addEmergencyAppointmentViewModel.SpecializedDoctor = _choosenDoctor.Username;
                    _addEmergencyAppointmentViewModel.StartDate = _currentTime.Date.ToString("yyyy-MM-dd");
                    DateTime dateTime = DateTime.MinValue + _currentTime.TimeOfDay;
                    _addEmergencyAppointmentViewModel.StartTime = dateTime.ToString(@"HH\:mm\:ss");

                }
                else
                {
                    _appointmentsForEachDoctor = _emergencyappointmentService.FilterAppointmentsBySpecialization(specializedDoctors);
                    _earliestTermForEachDoctor = _emergencyappointmentService.SortAppointmentTermsForEachDoctor(_appointmentsForEachDoctor, _currentTime, _twoHoursFromNow, _addEmergencyAppointmentViewModel.emergencyAppointmentDuration);


                    _choosenDoctor = _doctorService.FindDoctorWithEarliestTerm(_earliestTermForEachDoctor);
                    DateTime emergencyAppointmentDate = DateTime.MinValue;
                    if (_choosenDoctor != null)
                    {
                        emergencyAppointmentDate = _doctorService.FindDoctorsEarliestTerm(_earliestTermForEachDoctor, _choosenDoctor.Username);
                    }

                    if (_choosenDoctor != null && emergencyAppointmentDate != DateTime.MinValue)
                    {
                        _addEmergencyAppointmentViewModel.SpecializedDoctor = _choosenDoctor.Username;
                        _addEmergencyAppointmentViewModel.StartDate = emergencyAppointmentDate.Date.ToString("yyyy-MM-dd");
                        TimeSpan timeOfDay = emergencyAppointmentDate.TimeOfDay;
                        DateTime dateTime = DateTime.MinValue + timeOfDay;
                        _addEmergencyAppointmentViewModel.StartTime = dateTime.ToString(@"HH\:mm\:ss");
                    }
                    else
                    {
                        _addEmergencyAppointmentViewModel.isAllTermsTaken = true;
                        _addEmergencyAppointmentViewModel.potentialTakenTerms = _emergencyappointmentService.SortPotentialTakenTerms(_appointmentsForEachDoctor);
                        foreach (string appointmentId in _addEmergencyAppointmentViewModel.potentialTakenTerms.Keys)
                        {
                            _addEmergencyAppointmentViewModel.Terms.Add(_addEmergencyAppointmentViewModel.potentialTakenTerms[appointmentId]);
                        }
                    }
                }
            }
        }
    }
}
