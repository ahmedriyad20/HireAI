using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        public ApplicationRepository(HireAIDbContext db) : base(db) { }
    }
}
