using AutoMapper;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ApplicantRepository : GenericRepositoryAsync<Applicant>, IApplicantRepository
    {
        public ApplicantRepository(HireAIDbContext db) : base(db)
        { }

        public override async Task<Applicant?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.ApplicantSkills)
                    .ThenInclude(s => s.Skill)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public override IQueryable<Applicant> GetAll()
        {
            return _dbSet
                .Include(a => a.ApplicantSkills)
                    .ThenInclude(s => s.Skill);
        }
    }
}
