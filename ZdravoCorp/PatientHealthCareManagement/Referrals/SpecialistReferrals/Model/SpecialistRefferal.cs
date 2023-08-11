using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZdravoCorp.PatientHealthCareManagement.Referrals.SpecialistReferrals.Model
{
    public enum SpecialistReferralStatus
    {
        Created,
        Executed
    }
    public class SpecialistReferral : Referral
    {
        public SpecialistReferralStatus SpecialistReferralStatus { get; set; }
        public string ToDoctor { get; set; }

        public SpecialistReferral(string id,string patientUsername, string fromDoctor, string toDoctor, SpecialistReferralStatus status)
            : base(id, patientUsername, fromDoctor)

        {
            ToDoctor = toDoctor;
            SpecialistReferralStatus = status;
        }

    }
}
