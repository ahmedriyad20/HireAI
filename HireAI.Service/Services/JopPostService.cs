using AutoMapper;
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{

    public class JopPostService : IJobPostService

    {
        private readonly IMapper _mapper;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IJobSkillRepository _jobSkillRepository;

        public JopPostService(IMapper mapper , IJobPostRepository jobOpeningRepository , IJobSkillRepository jobSkillRepository)
        {
            _mapper = mapper;
            _jobPostRepository = jobOpeningRepository;
            _jobSkillRepository = jobSkillRepository;
        }

        public async Task CreateJobPostAsync(JobPostRequestDto jopOpeingRequestDto)
        {
            //save job 
            var createPostEntity = _mapper.Map<JobPost>(jopOpeingRequestDto);

            await _jobPostRepository.AddAsync(createPostEntity);

            //tie skills to job
            var skillIds = jopOpeingRequestDto.SkillIds;
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
            foreach ( var jobSkill in jobOpeningEntity.JobSkills)
            {
                await _jobSkillRepository.DeleteAsync(jobSkill);
            }
            //delete job post
            await _jobPostRepository.DeleteAsync(jobOpeningEntity);
        }

        public async Task<JobPostResponseDto> GetJobPostAsync(int id)
        {
           var jopPost = await _jobPostRepository.GetByIdAsync(id);
           
           if(jopPost == null)
           {
                  throw new Exception("Job Post not found");
           }

           var jobSkills = await _jobSkillRepository.GetSkillsByJobIdAsync(id);
            jopPost.JobSkills = jobSkills.ToList();
            return _mapper.Map<JobPostResponseDto>(jopPost);
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

        public async Task UpdateJobPostAsync(int id, JobPostRequestDto jobPostRequestDto)
        {
            if(jobPostRequestDto == null)
            {
                throw new ArgumentException(nameof(JobPostRequestDto));
            }

            if (jobPostRequestDto.SkillIds == null || !jobPostRequestDto.SkillIds.Any()) {

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
            await  _jobPostRepository.UpdateAsync(existing);

        }
    }
}
      
 
       
     



