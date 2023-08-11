using System.Collections.Generic;
using ZdravoCorp.CommunicatonManagement.Polls.Model;

namespace ZdravoCorp.CommunicatonManagement.Polls.Repository
{
    public interface IPollRepository
    {
        List<Poll> GetAll();
        Poll? GetById(string id);
        void Add(Poll poll);
        void Delete(Poll poll);
        void Update(Poll updatedPoll);
        string NextId();
    }
}
