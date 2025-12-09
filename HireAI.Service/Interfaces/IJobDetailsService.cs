using HireAI.Data.Helpers.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    /// <summary>
    /// Service for managing job details and applications
    /// </summary>
    public interface IJobDetailsService
    {
        /// <summary>
        /// Get all jobs for a specific HR
        /// </summary>
        Task<List<HRJobSummaryDto>> GetAllJobsForHRAsync(int hrId);

        /// <summary>
        /// Get detailed information about a job including all applications
        /// </summary>
        Task<JobDetailsDto> GetJobDetailsAsync(int jobId, int hrId);

        /// <summary>
        /// Get all applications for a specific job
        /// </summary>
        Task<List<JobApplicationDto>> GetJobApplicationsAsync(int jobId, int hrId);

        /// <summary>
        /// Get top applicants for a job (ranked by ATS score)
        /// </summary>
        Task<List<TopApplicantDto>> GetTopApplicantsAsync(int jobId, int hrId, int topCount = 10);

        /// <summary>
        /// Get top exam takers for a job (ranked by exam score)
        /// </summary>
        Task<List<TopExamTakerDto>> GetTopExamTakersAsync(int jobId, int hrId, int topCount = 10);
    }
}
