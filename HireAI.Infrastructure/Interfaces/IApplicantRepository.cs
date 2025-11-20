using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;

namespace HireAI.Infrastructure.GenericBase
{
    public interface IApplicantRepository : IRepository<Applicant>
    {
        // Add applicant-specific methods here if needed
    }
}
