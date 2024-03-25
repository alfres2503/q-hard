using src.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Repository
{
    public interface ICateringServiceRepository
    {
        Task<IEnumerable<CateringService>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy);
        Task<int> GetCount(string searchTerm);
        Task<CateringService> GetByID(int id);
        Task<CateringService> GetByEmail(string email);
        Task<CateringService> Create(CateringService catering_service);
        Task<CateringService> Update(int id, CateringService catering_service);
        Task<CateringService> ChangeState(int id);
        Task<bool> Delete(int id);
    }
}
