using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.Intrefaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ApplicantSkillRepository : GenericRepositoryAsync<ApplicantSkill>, IApplicantSkillRepository
    {
        public ApplicantSkillRepository(HireAIDbContext db) : base(db) { }
    }
}
