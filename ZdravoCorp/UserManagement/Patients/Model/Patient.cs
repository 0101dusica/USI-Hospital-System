using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model;

namespace ZdravoCorp.UserManagement.Patients.Model
{
    public class Patient : User
    {
        public MedicalRecord MedicalRecord { get; set; }

        public Patient() { }

        public Patient(User person, MedicalRecord medicalRecord)
        {
            FirstName = person.FirstName;
            LastName = person.LastName;
            Username = person.Username;
            Password = person.Password;
            UserStatus = person.UserStatus;
            MedicalRecord = medicalRecord;

        }
    }
}
