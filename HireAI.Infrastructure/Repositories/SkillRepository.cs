using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class SkillRepository : GenericRepositoryAsync<Skill>, ISkillRepository
    {
        

        public SkillRepository(HireAIDbContext context) : base(context)
        {

        }
    }
}
