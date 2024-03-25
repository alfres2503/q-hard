using src.Models;
using src.Repository;

namespace src.Services
{
    public class AssociateService : IAssociateService
    {
        private readonly IAssociateRepository _AssociateRepository;

        public AssociateService(IAssociateRepository AssociateRepository)
        {
            _AssociateRepository = AssociateRepository;
        }

        public async Task<IEnumerable<Associate>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy)
        {
            try { return await _AssociateRepository.GetAll(pageNumber, pageSize, searchTerm, orderBy).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
            
        }

        public async Task<int> GetCount(string searchTerm)
        {
            try { return await _AssociateRepository.GetCount(searchTerm).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Associate> GetByEmail(string email)
        {
            try { return await _AssociateRepository.GetByEmail(email).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Associate> GetByID(int id)
        {
            try { return await _AssociateRepository.GetByID(id).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public async Task<Associate> Create(Associate associate)
        {
            try { return await _AssociateRepository.Create(associate).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Associate> Update(int id, Associate associate)
        {
            try { return await _AssociateRepository.Update(id, associate); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try { return await _AssociateRepository.Delete(id); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Associate> ChangeState(int id)
        {
            try { return await _AssociateRepository.ChangeState(id); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Associate> GetByIdCard(string idCard)
        {
            try { return await _AssociateRepository.GetByIdCard(idCard); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

    }
}
