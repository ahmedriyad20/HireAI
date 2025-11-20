using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class HRRepository : GenericRepositoryAsync<HR>, IHRRepository
    {
        public HRRepository(HireAIDbContext db) : base(db)
        {
        }

        // Add HR-specific data access methods here
    }
}
