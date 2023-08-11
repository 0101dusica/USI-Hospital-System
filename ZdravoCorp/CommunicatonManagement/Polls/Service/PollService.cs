using System;
using System.Collections.Generic;
using ZdravoCorp.CommunicatonManagement.Polls.Repository;
using ZdravoCorp.CommunicatonManagement.Polls.Model;

namespace ZdravoCorp.CommunicatonManagement.Polls.Service
{
    public class PollService
    {
        private IPollRepository _pollRepository;
        public PollService(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public List<Poll> GetAll()
        {
            return _pollRepository.GetAll();
        }

        public Poll? GetById(string id)
        {
            return _pollRepository.GetById(id);
        }

        public void Add(Poll poll)
        {
            _pollRepository.Add(poll);
        }

        public void Delete(Poll poll)
        {
            _pollRepository.Delete(poll);
        }

        public void Update(Poll updatedPoll)
        {
            _pollRepository.Update(updatedPoll);
        }

        public string NextId()
        {
            return _pollRepository.NextId();
        }

    }
}
