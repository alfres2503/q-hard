using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;

namespace src.Repository
{
    public class CateringServiceRepository : ICateringServiceRepository
    {
        private readonly AppDBContext _context;

        public CateringServiceRepository(AppDBContext context)
        {
            _context = context;
        }

        // async method to get all Catering Services
        public async Task<IEnumerable<CateringService>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy)
        {
            try
            {
                var query = _context.CateringService.AsQueryable();

                // if there is a search term, filter the query
                if (!string.IsNullOrEmpty(searchTerm))
                    query = query.Where(cs => cs.Name.Contains(searchTerm) || cs.Phone.Contains(searchTerm) || cs.Email.Contains(searchTerm));

                // if there is an orderBy parameter, order the query
                switch (orderBy)
                {
                    case "id":
                        query = query.OrderBy(cs => cs.Id);
                        break;
                    case "id_desc":
                        query = query.OrderByDescending(cs => cs.Id);
                        break;
                    case "name":
                        query = query.OrderBy(cs => cs.Name);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(cs => cs.Name);
                        break;
                    case "email":
                        query = query.OrderBy(cs => cs.Email);
                        break;
                    case "email_desc":
                        query = query.OrderByDescending(cs => cs.Email);
                        break;
                    case "role":
                        query = query.OrderBy(cs => cs.Phone);
                        break;
                    case "role_desc":
                        query = query.OrderByDescending(cs => cs.Phone);
                        break;
                    case "active":
                        query = query.OrderBy(cs => cs.IsActive);
                        break;
                    case "active_desc":
                        query = query.OrderByDescending(cs => cs.IsActive);
                        break;
                    default:
                        query = query.OrderBy(cs => cs.Id);
                        break;
                }

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(cs => new CateringService
                    {
                        Id = cs.Id,
                        Name = cs.Name,
                        Email = cs.Email,
                        Phone = cs.Phone,
                        IsActive = cs.IsActive,
                    })
                    .ToListAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<int> GetCount(string searchTerm)
        {
            try
            {
                // if there is a search term, return the count of the filtered query
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var query = _context.CateringService.AsQueryable();
                    return await query.Where(cs => cs.Name.Contains(searchTerm) || cs.Phone.Contains(searchTerm) || cs.Email.Contains(searchTerm)).CountAsync();
                }

                return await _context.CateringService.CountAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<CateringService> GetByEmail(string email)
        {
            try
            {
                return await _context.CateringService
                    .FirstOrDefaultAsync(m => m.Email == email);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<CateringService> GetByID(int id)
        {
            try
            {
                return await _context.CateringService
                    .Select(cs => new CateringService
                    {
                        Id = cs.Id,
                        Name = cs.Name,
                        Email = cs.Email,
                        Phone = cs.Phone,
                        IsActive = cs.IsActive,
                    })
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<CateringService> Create(CateringService catering_service)
        {
            try
            {
                await _context.CateringService.AddAsync(catering_service);
                await _context.SaveChangesAsync();

                if (this.GetByID(catering_service.Id) != null)
                    return catering_service;
                else
                    throw new Exception("Catering Service not created");
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            bool deletedStatus = false;
            try
            {
                CateringService catering_service = await _context.CateringService.FindAsync(id);

                if (catering_service == null)
                    return deletedStatus;

                _context.CateringService.Remove(catering_service);
                await _context.SaveChangesAsync();

                catering_service = await _context.CateringService.FindAsync(id);
                if (catering_service == null)
                    deletedStatus = true;

                return deletedStatus;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<CateringService> ChangeState(int id)
        {
            try
            {
                _context.CateringService.Find(id).IsActive = !_context.CateringService.Find(id).IsActive;
                await _context.SaveChangesAsync();
                return await _context.CateringService.FindAsync(id);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }

        }

        public async Task<CateringService> Update(int id, CateringService catering_service)
        {
            try
            {
                _context.CateringService.Find(id).Name = catering_service.Name;
                _context.CateringService.Find(id).Phone = catering_service.Phone;
                _context.CateringService.Find(id).Email = catering_service.Email;
                _context.CateringService.Find(id).IsActive = catering_service.IsActive;

                await _context.SaveChangesAsync();
                return await _context.CateringService
                   .Select(cs => new CateringService
                   {
                       Id = cs.Id,
                       Name = cs.Name,
                       Email = cs.Email,
                       Phone = cs.Phone,
                       IsActive = cs.IsActive,
                   })
                   .FirstOrDefaultAsync(cs => cs.Id == id);
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception($"Database error: {dbEx.Message}", dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

    }
}
