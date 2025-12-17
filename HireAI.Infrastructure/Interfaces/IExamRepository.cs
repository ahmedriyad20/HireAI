using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.Intrefaces
{
    public interface IExamRepository : IGenericRepositoryAsync<Exam> {
        public Task<Exam?> GetExamByApplicanIdAsync(int applicantId);
        public  Task<ICollection<Exam>?> GetExamsByApplicantIdAsync(int applicantId, int pageNumber = 1, int pageSize = 5);
        public Task CreateExamAsncy(Exam exam);
        public Task<Exam?> GetExamByJobIdAsync(int jobId);

    }
}
