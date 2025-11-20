using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class ApplicantResponseRepository : GenericRepositoryAsync<ApplicantResponse>, IApplicantResponseRepository
    {
        public ApplicantResponseRepository(HireAIDbContext db) : base(db) { }
    }
}
