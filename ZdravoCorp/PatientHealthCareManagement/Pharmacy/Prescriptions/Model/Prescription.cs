using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.PatientHealthCareManagement.Pharmacy.Prescriptions.Model
{

    public enum PrescriptionStatus
    {
        NotShowed,
        Showed
    }
    public enum Instruction
    {
        WhileEating,
        AfterEating,
        BeforeEating,
        DoesNotMatter
    }
    public class Prescription
    {
        public string Id { get; set; }
        public int DailyUsage { get; set; }
        public Instruction Instruction { get; set; }
        public string MedicineId { get; set; }

        public TimeSlot TimeSlot { get; set; }
        public string PatientUsername { get; set; }
        public string DoctorUsername { get; set; }
        public TimeSpan Time { get; set; }
        public int TimeSet { get; set; }

        public PrescriptionStatus PrescriptionStatus { get; set; }


        public Prescription() { }

        public Prescription(string id, int dailyUsage, Instruction instruction, TimeSpan time, string patientUsername, string doctorUsername, TimeSlot timeslot, string medicineId)
        {
            Id = id;
            DailyUsage = dailyUsage;
            Instruction = instruction;
            TimeSlot = timeslot;
            Time = time;
            PatientUsername = patientUsername;
            DoctorUsername = doctorUsername;
            MedicineId = medicineId;
            TimeSet = 60;
            PrescriptionStatus = PrescriptionStatus.NotShowed;
        }

    }
}
