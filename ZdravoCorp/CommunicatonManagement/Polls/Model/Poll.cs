using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ZdravoCorp.CommunicatonManagement.Polls.Model
{
    public enum PollType
    {
        Hospital,
        Doctor
    }
    public class Poll
    {
        public string Id { get; set; }
        public string PatientUsername { get; set; }
        public PollType PollType { get; set; }
        public List<string> Questions { get; set; }
        public string Comment { get; set; }
        public List<int> PollGrades { get; set; }
        public string? DoctorUsername { get; set; }
        public double AverageGrade => PollGrades?.Average() ?? 0;



        [JsonConstructor]
        public Poll(string id, string patientUsername, PollType pollType, List<string> questions, string comment, List<int> pollGrades, string doctorUsername)
        {
            this.Id = id;
            this.PatientUsername = patientUsername;
            this.PollType = pollType;
            this.Questions = questions;
            this.Comment = comment;
            this.PollGrades = pollGrades;
            this.DoctorUsername = doctorUsername;

        }
    }

}
