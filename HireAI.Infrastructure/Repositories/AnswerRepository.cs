using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HireAI.Infrastructure.Repositories
{
    public class AnswerRepository : GenericRepositoryAsync<Answer>, IAnswerRepository
    {
        public AnswerRepository(HireAIDbContext db) : base(db) { 
        
        
        }
    }
}
