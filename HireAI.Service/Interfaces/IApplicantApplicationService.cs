using HireAI.Data.DTOs.ApplicantDashboard;
using HireAI.Data.Helpers.DTOs.ApplicantApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IApplicantApplicationService
    {
        public Task<IEnumerable<ApplicantApplicationsListDto>> GetApplicantApplicationsList(int applicantId);

        public Task<ApplicationDetailsDto> GetApplicationDetailsAsync(int applicationId, int applicantId);
    }
}
