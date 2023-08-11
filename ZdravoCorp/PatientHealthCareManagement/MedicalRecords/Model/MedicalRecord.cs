using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.PatientHealthCareManagement.MedicalRecords.Model
{
    public class MedicalRecord
    {
        public int Height { get; set; }
        public int Weight { get; set; }
        public List<string> MedicalHistory { get; set; }
        public List<string> Allergies { get; set; }

        public List<string> AppointmentIds { get; set; }

        public MedicalRecord()
        {
            MedicalHistory = new List<string>();
            Allergies = new List<string>();
        }

        public MedicalRecord(int height, int weight, List<string> medicalHistory, List<string> allergies, List<string> appointmentIds)
        {
            Height = height;
            Weight = weight;
            MedicalHistory = medicalHistory;
            Allergies = allergies;
            AppointmentIds = appointmentIds;

        }


    }

}
