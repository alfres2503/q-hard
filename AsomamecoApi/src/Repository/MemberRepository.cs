﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using src.Models;
using src.Utils;

namespace src.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly AppDBContext _context;

        public MemberRepository(AppDBContext context)
        {
            _context = context;
        }

        // async method to get all members
        public async Task<IEnumerable<Member>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy)
        {
            try
            {
                var query = _context.Member.AsQueryable();

                // if there is a search term, filter the query
                if (!string.IsNullOrEmpty(searchTerm))
                    query = query.Where(m => m.FirstName.Contains(searchTerm) || m.LastName.Contains(searchTerm) || m.Email.Contains(searchTerm));

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
                        query = query.OrderBy(m => m.FirstName).ThenBy(m => m.LastName);
                        break;
                    case "name_desc":
                        query = query.OrderByDescending(m => m.FirstName).ThenByDescending(m => m.LastName);
                        break;
                    case "email":
                        query = query.OrderBy(m => m.Email);
                        break;
                    case "email_desc":
                        query = query.OrderByDescending(m => m.Email);
                        break;
                    case "role":
                        query = query.OrderBy(m => m.IdRole);
                        break;
                    case "role_desc":
                        query = query.OrderByDescending(m => m.IdRole);
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
                    .Select(m => new Member
                    {
                        Id = m.Id,
                        IdRole = m.IdRole,
                        IdCard = m.IdCard,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Email = m.Email,
                        IsActive = m.IsActive,
                        Role = m.Role
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
                    var query = _context.Member.AsQueryable();
                    return await query.Where(m => m.FirstName.Contains(searchTerm) || m.LastName.Contains(searchTerm) || m.Email.Contains(searchTerm)).CountAsync();
                }

                return await _context.Member.CountAsync();
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

        public async Task<Member> GetByEmail(string email)
        {
            try
            {
                return await _context.Member
                    .Include(m => m.Role)
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

        public async Task<Member> GetByID(int id)
        {
            try
            {
                return await _context.Member
                    .Include(m => m.Role)
                    .Select(m => new Member
                    {
                        Id = m.Id,
                        IdRole = m.IdRole,
                        IdCard = m.IdCard,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Email = m.Email,
                        IsActive = m.IsActive,
                        Role = m.Role
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

        public async Task<Member> Create(Member member)
        {
            try
            {
                member.Password = Cryptography.EncryptAES(member.Password);

                await _context.Member.AddAsync(member);
                await _context.SaveChangesAsync();

                return member; 
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
                Member member = await _context.Member.FindAsync(id);

                if (member == null)
                    return deletedStatus;

                _context.Member.Remove(member);
                await _context.SaveChangesAsync();

                member = await _context.Member.FindAsync(id);
                if (member == null)
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

        public async Task<Member> ChangeState(int id)
        {
            try
            {
                _context.Member.Find(id).IsActive = !_context.Member.Find(id).IsActive;
                await _context.SaveChangesAsync();
                return await _context.Member.FindAsync(id);
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

        public async Task<Member> Update(int id,Member member)
        {
            try
            {
                _context.Member.Find(id).IdRole = member.IdRole;
                _context.Member.Find(id).IdCard = member.IdCard;
                _context.Member.Find(id).FirstName = member.FirstName;
                _context.Member.Find(id).LastName = member.LastName;
                _context.Member.Find(id).Email = member.Email;
                _context.Member.Find(id).IsActive = member.IsActive;

                if (member.Password != null)
                    _context.Member.Find(id).Password = member.Password;

                await _context.SaveChangesAsync();
                return await _context.Member
                   .Include(m => m.Role)
                   .Select(m => new Member
                   {
                       Id = m.Id,
                       IdRole = m.IdRole,
                       IdCard = m.IdCard,
                       FirstName = m.FirstName,
                       LastName = m.LastName,
                       Email = m.Email,
                       IsActive = m.IsActive,
                       Role = m.Role
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
        
        public async Task<Member> GetByIdCard(string idCard)
        {
            try
            {
                return await _context.Member
                    .Include(m => m.Role)
                    .Select(m => new Member
                    {
                        Id = m.Id,
                        IdRole = m.IdRole,
                        IdCard = m.IdCard,
                        FirstName = m.FirstName,
                        LastName = m.LastName,
                        Email = m.Email,
                        IsActive = m.IsActive,
                        Role = m.Role
                    })
                    .FirstOrDefaultAsync(m => m.IdCard == idCard);
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
