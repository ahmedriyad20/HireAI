using HireAI.Data.Helpers.DTOs.JobOpening.Request;
using HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response;

public interface IJobPostService
{
    public Task<JobPostResponseDto> GetJobPostAsync(int id);
    public Task CreateJobPostAsync(JobPostRequestDto JobOpeingRequestDto);
    public Task DeleteJobPostAsync(int id);
    public Task<ICollection<JobPostResponseDto>> GetJobPostForHrAsync(int hrid);
    public Task UpdateJobPostAsync(int id, JobPostRequestDto JobOpeingRequestDto);
    public Task<int> GetTotalApplicationsByJobIdAsync(int jobId);
    public Task<ICollection<JobPostResponseDto>> GetAllJobPostsAsync();
}