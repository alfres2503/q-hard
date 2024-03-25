using src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Repository
{
    public interface IAssociateRepository
    {
        Task<IEnumerable<Associate>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy);
        Task<int> GetCount(string searchTerm);
        Task<Associate> GetByID(int id);
        Task<Associate> GetByEmail(string email);
        Task<Associate> Create(Associate associate);
        Task<Associate> GetByIdCard(string idCard);
        Task<Associate> Update(int id, Associate associate);
        Task<Associate> ChangeState(int id);
        Task<bool> Delete(int id);
    }
}
