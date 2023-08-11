using System.Collections.Generic;
using ZdravoCorp.FacilitiesManagement.Transfers.Model;

namespace ZdravoCorp.FacilitiesManagement.Transfers.Repository
{
    public interface ITransferRepository
    {
        List<Transfer> GetAll();
        Transfer? GetById(string id);
        void Add(Transfer transfer);
        void Delete(Transfer transfer);
        void Update(Transfer updatedTransfer);
        string NextId();
    }
}
