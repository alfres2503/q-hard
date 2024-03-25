using src.Models;

namespace src.Services
{
    public interface IAssociateService
    {
        Task<IEnumerable<Associate>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy);
        Task<int> GetCount(string searchTerm);
        Task<Associate> GetByID(int id);
        Task<Associate> GetByIdCard(string idCard);
        Task<Associate> GetByEmail(string email);
        Task<Associate> Create(Associate associate);
        Task<Associate> Update(int id, Associate associate);
        Task<Associate> ChangeState(int id);
        Task<bool> Delete(int id);
    }
}
