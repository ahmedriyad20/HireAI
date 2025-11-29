
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IJopService
    {
        public Task<JobPostResponseDto> GetJobPostAsync(int id);
        public Task CreateJobPostAsync(JobPostRequestDto jopOpeingRequestDto);
        public Task DeleteJobPostAsync(int id);
        public Task<ICollection<JobPostResponseDto>> GetJobPostForHrAsync(int hrid);
        public Task UpdateJobPostAsync(int id, JobPostRequestDto jopOpeingRequestDto);
    }
}
