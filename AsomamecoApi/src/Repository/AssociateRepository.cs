using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Utils;
using System.ComponentModel.DataAnnotations;

namespace src.Repository
{
    public class AssociateRepository : IAssociateRepository
    {
        private readonly AppDBContext _context;

        public AssociateRepository(AppDBContext context)
        {
            _context = context;
        }

        // async method to get all associates
        public async Task<IEnumerable<Associate>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy)
        {
            try
            {
                var query = _context.Associate.AsQueryable();

                // if there is a search term, filter the query
                if (!string.IsNullOrEmpty(searchTerm))
                    query = query.Where(m => m.Name.Contains(searchTerm) || m.Email.Contains(searchTerm));

                // if there is an orderBy parameter, order the query
                switch (orderBy)
                {
                    case "id":
                        query = query.OrderBy(m => m.Id);
                        break;
                    case "id_desc":
                        query = query.OrderByDescending(m => m.Id);
                        break;
                    case "name":
                        query = query.OrderBy(m => m.Name);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(m => m.Name);
                        break;
                    case "email":
                        query = query.OrderBy(m => m.Email);
                        break;
                    case "email_desc":
                        query = query.OrderByDescending(m => m.Email);
                        break;
                    case "active":
                        query = query.OrderBy(m => m.IsActive);
                        break;
                    case "active_desc":
                        query = query.OrderByDescending(m => m.IsActive);
                        break;
                    default:
                        query = query.OrderBy(m => m.Id);
                        break;
                }

                return await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(a => new Associate
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Email = a.Email,
                        Phone = a.Phone,
                        IsActive = a.IsActive,
                        IdCard = a.IdCard,
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
                return await _context.Associate.CountAsync();
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

        public async Task<Associate> GetByEmail(string email)
        {
            try
            {
                return await _context.Associate
                    .FirstOrDefaultAsync(a => a.Email == email);
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

        public async Task<Associate> GetByID(int id)
        {
            try
            {
                return await _context.Associate
                    .Select(a => new Associate
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Email = a.Email,
                        Phone = a.Phone,
                        IsActive = a.IsActive,
                        IdCard = a.IdCard,
                    })
                    .FirstOrDefaultAsync(a => a.Id == id);
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

        public async Task<Associate> Create(Associate associate)
        {
            try
            {
                await _context.Associate.AddAsync(associate);
                await _context.SaveChangesAsync();

                return associate;
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
                Associate associate = await _context.Associate.FindAsync(id);

                if (associate == null)
                    return deletedStatus;

                _context.Associate.Remove(associate);
                await _context.SaveChangesAsync();

                associate = await _context.Associate.FindAsync(id);
                if (associate == null)
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

        public async Task<Associate> ChangeState(int id)
        {
            try
            {
                _context.Associate.Find(id).IsActive = !_context.Associate.Find(id).IsActive;
                await _context.SaveChangesAsync();
                return await _context.Associate.FindAsync(id);
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

        public async Task<Associate> Update(int id, Associate associate)
        {
            try
            {
                //_context.Entry(associate).State = EntityState.Modified;
                //await _context.SaveChangesAsync();
                //return associate;

                _context.Associate.Find(id).Name = associate.Name;
                _context.Associate.Find(id).Email = associate.Email;
                _context.Associate.Find(id).IsActive = associate.IsActive;
                _context.Associate.Find(id).IdCard = associate.IdCard;
                _context.Associate.Find(id).Phone = associate.Phone;

                await _context.SaveChangesAsync();
                return await _context.Associate
                   .Select(a => new Associate
                   {
                       Id = a.Id,
                       Name = a.Name,
                       Email = a.Email,
                       Phone = a.Phone,
                       IsActive = a.IsActive,
                       IdCard = a.IdCard,
                   })
                   .FirstOrDefaultAsync(a => a.Id == id);
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

        public async Task<Associate> GetByIdCard(string idCard)
        {
            try
            {
                return await _context.Associate
                    .Select(a => new Associate
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Email = a.Email,
                        Phone = a.Phone,
                        IsActive = a.IsActive,
                        IdCard = a.IdCard,
                    })
                    .FirstOrDefaultAsync(a => a.IdCard == idCard);
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
