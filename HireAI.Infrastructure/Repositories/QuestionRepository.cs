using HireAI.Data.Models;
using HireAI.Infrastructure.Context;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepositoryAsync<Question>, IQuestionRepository
    {
        public QuestionRepository(HireAIDbContext db) : base(db) { }
    }
}
