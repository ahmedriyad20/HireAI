using HireAI.Data.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Applicant.Request
{
    /// <summary>
    /// DTO used to update an existing Applicant.
    /// Contains fields that can be modified after creation.
    /// </summary>
    public class ApplicantUpdateDto
    {
        [StringLength(200)]
        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public DateOnly? DateOnly { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [Url]
        public string? ResumeUrl { get; set; }

        public enSkillLevel? SkillLevel { get; set; }

        public int? CVId { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsPremium { get; set; }
    }
}