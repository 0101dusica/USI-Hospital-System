using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.UserManagement.Doctors.Model;
using ZdravoCorp.UserManagement.Doctors.Service;
using ZdravoCorp.UserManagement.Doctors.ViewModel;
using ZdravoCorp.Utils.Command;

namespace ZdravoCorp.UserManagement.Doctors.Command
{
    public class OrderByAverageGradeCommand : BaseCommand
    {

        public DoctorService DoctorService { get; set; }
        public PatientSearchDoctorsViewModel PatientSearchDoctorsViewModel { get; set; }
        public OrderByAverageGradeCommand(PatientSearchDoctorsViewModel patientSearchDoctorsViewModel, DoctorService doctorService)
        {
            PatientSearchDoctorsViewModel = patientSearchDoctorsViewModel;
            DoctorService = doctorService;

        }
        public override void Execute(object? parameter)
        {
            PatientSearchDoctorsViewModel.Doctors = new ObservableCollection<Doctor>(
                DoctorService.OrderByAverageGrade(PatientSearchDoctorsViewModel.Doctors));
        }
    }

}
