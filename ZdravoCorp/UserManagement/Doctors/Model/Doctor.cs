using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.UserManagement.Doctors.Model
{
    public enum Specialization
    {
        GeneralPractitioner,
        Cardiologist,
        Dermatologist,
        Neurologist,
        Pediatrician

    }
    public class Doctor : User
    {

        public Specialization Specialization { get; set; }
        public List<string> AppointmentIds { get; set; } //promeniti
        public List<string> FreeDaysIds { get; set; } //?
        public IList<int> Grades { get; set; }
        public double AverageGrade => Grades?.Average() ?? 0;

        public Doctor() { }

        public Doctor(User person, Specialization specialization, List<string> appointmentIds, List<string> freeDaysIds)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Username = person.Username;
            Password = person.Password;
            UserStatus = person.UserStatus;
            Specialization = specialization;
            AppointmentIds = appointmentIds;
            FreeDaysIds = freeDaysIds;
        }
    }
}
