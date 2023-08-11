using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.PatientHealthCareManagement.Hospitalization.HospitalCares.Model
{

    public enum HospitalCareStatus
    {
        InProgress,
        Finished
    }

    public class HospitalCare
    {
      
        public string Id { get; set; }
        public string ReferralId { get; set; }
        public string PatientUsername { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public string RoomId { get; set; }
        public string Therapy { get; set; }
        public List<string> VisitIds { get; set; }
        public HospitalCareStatus HospitalCareStatus { get; set; }


        public HospitalCare() { }

        public HospitalCare(string id, string referralId, string patientUsername, TimeSlot timeSlot, string roomId, string therapy, List<string> visitIds, HospitalCareStatus hospitalCareStatus)
        {
            Id = id;
            ReferralId = referralId;
            PatientUsername = patientUsername;
            TimeSlot = timeSlot;
            RoomId = roomId;
            Therapy = therapy;
            VisitIds = visitIds;
            HospitalCareStatus = hospitalCareStatus;
        }
    }
}
