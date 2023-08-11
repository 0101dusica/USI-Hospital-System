using System;
using System.Collections.Generic;
using System.Linq;
using ZdravoCorp.Utils.Serializer;

using ZdravoCorp.CommunicatonManagement.Polls.Model;

namespace ZdravoCorp.CommunicatonManagement.Polls.Repository
{
    public class PollRepository : IPollRepository
    {
        private static List<Poll> _polls = new List<Poll>();
        private const string _storagePath = "../../../Data/Polls.json";

        private ISerializer<Poll> _serializer;


        public PollRepository(ISerializer<Poll> serializer)
        {
            _serializer = serializer;
            _polls = Load();
        }

        public List<Poll> Load()
        {
            return _serializer.FromJson(_storagePath);
        }

        public void Save()
        {
            _serializer.ToJson(_storagePath, _polls);
        }

        public List<Poll> GetAll()
        {
            return _polls;
        }

        public Poll? GetById(string id)
        {
            return _polls.FirstOrDefault(p => p.Id.Equals(id));
        }


        public void Add(Poll poll)
        {
            _polls.Add(poll);
            Save();
        }

        public void Delete(Poll poll)
        {
            _polls.Remove(poll);
            Save();
        }

        public void Update(Poll updatedPoll)
        {
            var existingPoll = GetById(updatedPoll.Id);
            if (existingPoll != null)
            {
                existingPoll.PatientUsername = updatedPoll.PatientUsername;
                existingPoll.PollType = updatedPoll.PollType;
                existingPoll.Questions = updatedPoll.Questions;

                Save();
            }
        }

        public string NextId()
        {
            string? lastId = _polls.LastOrDefault()?.Id;
            if (lastId == null)
            {
                return "poll1";
            }
            else
            {
                int lastIdNumber = int.Parse(lastId.Replace("poll", ""));
                return $"poll{lastIdNumber + 1}";
            }
        }
    }
}
