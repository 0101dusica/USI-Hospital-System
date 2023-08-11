using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZdravoCorp.SchedulingManagement;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.HospitalCareReferrals.Model
{
    public class HospitalCareReferral : Referral
    {
        public TimeSlot TimeSlot { get; set; }
        public List<string> AdditionalTests { get; set; }
        public string Therapy { get; set; }

        public HospitalCareReferral(string id, string patientUsername, string fromDoctor,TimeSlot timeSlot, List<string> additionalTests, string therapy)
            : base(id,patientUsername,fromDoctor)
        {
            TimeSlot = timeSlot;
            AdditionalTests = additionalTests;
            Therapy = therapy;
        }

    }
}
