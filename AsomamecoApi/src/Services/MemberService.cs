﻿using src.Models;
using src.Repository;

namespace src.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<Member>> GetAll(int pageNumber, int pageSize, string searchTerm, string orderBy)
        {
            try { return await _memberRepository.GetAll(pageNumber, pageSize, searchTerm, orderBy).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
            
        }

        public async Task<int> GetCount(string searchTerm)
        {
            try { return await _memberRepository.GetCount(searchTerm).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Member> GetByEmail(string email)
        {
            try { return await _memberRepository.GetByEmail(email).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Member> GetByID(int id)
        {
            try { return await _memberRepository.GetByID(id).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }
        public async Task<Member> Create(Member member)
        {
            try { return await _memberRepository.Create(member).ConfigureAwait(false); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Member> Update(int id, Member member)
        {
            try { return await _memberRepository.Update(id, member); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try { return await _memberRepository.Delete(id); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Member> ChangeState(int id)
        {
            try { return await _memberRepository.ChangeState(id); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        public async Task<Member> GetByIdCard(string idCard)
        {
            try { return await _memberRepository.GetByIdCard(idCard); }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

    }
}
