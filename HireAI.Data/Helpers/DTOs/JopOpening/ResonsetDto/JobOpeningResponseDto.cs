using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.JopOpening.ResonsetDto
{
    /// <summary>
    /// DTO used for returning JobOpening information to the client (GET response)
    /// </summary>
    public class JobOpeningResponseDto
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

        public int HRId { get; set; }  // Usually returned for reference

        // Optional: include associated skill names instead of IDs
        public IEnumerable<string>? Skills { get; set; }

        public DateTime CreatedAt { get; set; }  // Useful for listing/sorting
    }

}
