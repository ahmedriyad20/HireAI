using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response
{
    using global::HireAI.Data.Helpers.DTOs.SkillDtos;
    using global::HireAI.Data.Helpers.Enums;
   
    using System;
    using System.Collections.Generic;

    namespace HireAI.Data.Helpers.DTOs.JobOpeningDtos.Response
    {
        /// <summary>
        /// DTO used to return a JobPost response.
        /// Contains all job opening details with metadata for API responses.
        /// </summary>
        public class JobPostResponseDto
        {
            public int Id { get; set; }

            public string Title { get; set; } = null!;

            public string? CompanyName { get; set; }

            public string? Description { get; set; }

            public enJobStatus JobStatus { get; set; }

            public int? ExamDurationMinutes { get; set; }

            public enExperienceLevel? ExperienceLevel { get; set; }

            public enEmploymentType? EmploymentType { get; set; }

            public string? Location { get; set; }

            public string? SalaryRange { get; set; }

            public int? NumberOfQuestions { get; set; }

            public DateTime? ApplicationDeadline { get; set; }

            public int? ATSMinimumScore { get; set; }

            public bool AutoSend { get; set; }

            public int? HRId { get; set; }

            public string? HRName { get; set; }

            public ICollection<SkillResponseDto>? Skills { get; set; }

            public DateTime CreatedAt { get; set; }

            public DateTime? UpdatedAt { get; set; }

            public int TotalApplications { get; set; }

            public int ExamsCompleted { get; set; }
        }
    }
}
