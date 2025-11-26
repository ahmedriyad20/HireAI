using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IExamRepository : IGenericRepositoryAsync<Exam> {
        public Task<Exam?> GetExamByApplicanIdAsync(int applicantId);
        public  Task<ICollection<Exam>?> GetExamsByApplicantIdAsync(int applicantId, int pageNumber = 1, int pageSize = 5);

    }
}
