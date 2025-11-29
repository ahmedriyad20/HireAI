using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.JobPostDtos
{

        /// <summary>
        /// DTO used to update an existing JobPost.
        /// Contains fields that can be modified after creation.
        /// </summary>
        public class JobPostUpdateDto
        {
            [MaxLength(200)]
            public string? Title { get; set; }

            [MaxLength(200)]
            public string? CompanyName { get; set; }

            [MaxLength(3000)]
            public string? Description { get; set; }

            public enJobStatus? JobStatus { get; set; }

            public int? ExamDurationMinutes { get; set; }

            public enExperienceLevel? ExperienceLevel { get; set; }

            public enEmploymentType? EmploymentType { get; set; }

            [MaxLength(200)]
            public string? Location { get; set; }

            [MaxLength(50)]
            public string? SalaryRange { get; set; }

            public int? NumberOfQuestions { get; set; }

            public DateTime? ApplicationDeadline { get; set; }

            public int? ATSMinimumScore { get; set; }

            public bool? AutoSend { get; set; }

            public IEnumerable<int>? SkillIds { get; set; }
        }
    }

