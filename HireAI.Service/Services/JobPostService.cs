using AutoMapper;
using HireAI.Data.Helpers.DTOs.JobOpening.Request;
using HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HireAI.Service.Services
{

    public class JobPostService : IJobPostService

    {
        private readonly IMapper _mapper;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IJobSkillRepository _jobSkillRepository;

        public JobPostService(IMapper mapper, IJobPostRepository jobOpeningRepository, IJobSkillRepository jobSkillRepository)
        {
            _mapper = mapper;
            _jobPostRepository = jobOpeningRepository;
            _jobSkillRepository = jobSkillRepository;
        }

        public async Task CreateJobPostAsync(JobPostRequestDto JobOpeingRequestDto)
        {
            //save job 
            var createPostEntity = _mapper.Map<JobPost>(JobOpeingRequestDto);

            await _jobPostRepository.AddAsync(createPostEntity);

            //tie skills to job
            var skillIds = JobOpeingRequestDto.SkillIds;
            if (skillIds != null && skillIds.Any())
            {
                for (int i = 0; i < skillIds.Count(); i++)
                {
                    var jobSkillEntity = new JobSkill
                    {
                        JobId = createPostEntity.Id,
                        SkillId = skillIds.ElementAt(i)
                    };
                    await _jobSkillRepository.AddAsync(jobSkillEntity); ;
                }
            }
        }


        public async Task DeleteJobPostAsync(int id)
        {
            var jobOpeningEntity = await _jobPostRepository.GetByIdAsync(id);
            if (jobOpeningEntity == null)
            {
                throw new Exception("Job Opening not found");
            }
            //delete associated skills  
            foreach (var jobSkill in jobOpeningEntity.JobSkills)
            {
                await _jobSkillRepository.DeleteAsync(jobSkill);
            }
            //delete job post
            await _jobPostRepository.DeleteAsync(jobOpeningEntity);
        }

        public async Task<JobPostResponseDto> GetJobPostAsync(int id)
        {
            var JobPost = await _jobPostRepository.GetByIdAsync(id);

            if (JobPost == null)
            {
                throw new Exception("Job Post not found");
            }

            var jobSkills = await _jobSkillRepository.GetSkillsByJobIdAsync(id);
            JobPost.JobSkills = jobSkills.ToList();
            return _mapper.Map<JobPostResponseDto>(JobPost);
        }

        public async Task<ICollection<JobPostResponseDto>> GetJobPostForHrAsync(int hrid)
        {
            var jobOpenings = await _jobPostRepository.GetJobPostForHrAsync(hrid);

            if (jobOpenings == null || !jobOpenings.Any())
            {
                return Array.Empty<JobPostResponseDto>();
            }
            return _mapper.Map<ICollection<JobPostResponseDto>>(jobOpenings);
        }

        //public async Task<ICollection<JobPostResponseDto>> GetJobPostForHrAsync(int hrid)
        //{
        //    var jobOpenings = await _jobPostRepository.GetJobPostForHrAsync(hrid);

        //    if (jobOpenings == null || !jobOpenings.Any())
        //    {
        //        return Array.Empty<JobPostResponseDto>();
        //    }

        //    var jobPostDtos = _mapper.Map<ICollection<JobPostResponseDto>>(jobOpenings);

        //    // Calculate TotalApplications and ExamsCompleted in the service
        //    foreach (var jobPostDto in jobPostDtos)
        //    {
        //        var jobPost = jobOpenings.FirstOrDefault(j => j.Id == jobPostDto.Id);
        //        if (jobPost != null)
        //        {
        //            jobPostDto.TotalApplications = jobPost.Applications?.Count ?? 0;
        //            jobPostDto.ExamsCompleted = jobPost.Applications?
        //                .Count(a => a.ExamStatus == enExamStatus.Completed) ?? 0;
        //        }
        //    }

        //    return jobPostDtos;
        //}

        public async Task UpdateJobPostAsync(int id, JobPostRequestDto jobPostRequestDto)
        {
            if (jobPostRequestDto == null)
            {
                throw new ArgumentException(nameof(JobPostRequestDto));
            }

            if (jobPostRequestDto.SkillIds == null || !jobPostRequestDto.SkillIds.Any())
            {

                jobPostRequestDto.SkillIds = new List<int>();
            }

            var existing = await _jobPostRepository.GetByIdAsync(id);

            if (existing == null)
                throw new Exception("Job Opening not found");


            _mapper.Map(jobPostRequestDto, existing);

            //compute what to add adn what to remove
            var currentSkillIds = existing.JobSkills.Select(js => js.SkillId).ToList();
            var newSkillIds = jobPostRequestDto.SkillIds.ToList();

            var toAdd = newSkillIds.Except(currentSkillIds).ToList();
            var toRemove = currentSkillIds.Except(newSkillIds).ToList();

            if (toAdd.Any())
            {
                foreach (var skillId in toAdd)
                {
                    var jobSkillEntity = new JobSkill
                    {
                        JobId = existing.Id,
                        SkillId = skillId
                    };
                    await _jobSkillRepository.AddAsync(jobSkillEntity);
                }
            }
            //to remove
            if (toRemove.Any())
            {
                foreach (var skillId in toRemove)
                {
                    var jobSkillEntity = existing.JobSkills.FirstOrDefault(js => js.SkillId == skillId);
                    if (jobSkillEntity != null)
                    {
                        await _jobSkillRepository.DeleteAsync(jobSkillEntity);
                    }
                }
            }
            //update job post
            await _jobPostRepository.UpdateAsync(existing);

        }

        public Task<int> GetTotalApplicationsByJobIdAsync(int jobId)
        {
            return _jobPostRepository.GetTotalApplicationsByJobIdAsync(jobId);
        }

        public async Task<ICollection<JobPostResponseDto>> GetAllJobPostsAsync()
        {
            var jobs = await _jobPostRepository.GetAll().
                AsNoTracking()
                .Include(j => j.HR)
                .Include(j => j.Applications)
                .ToListAsync();
            var jobdto = _mapper.Map<ICollection<JobPostResponseDto>>(jobs);
            return jobdto;
        }
    }
}
      
 
       
     



