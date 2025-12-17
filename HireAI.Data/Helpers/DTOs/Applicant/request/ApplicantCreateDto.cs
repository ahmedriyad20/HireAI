using HireAI.Data.Helpers.Enums;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace HireAI.Data.Helpers.DTOs.Applicant.Request
{
    /// <summary>
    /// DTO used to create a new Applicant.
    /// Contains fields required for applicant registration.
    /// </summary>
    public class ApplicantCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = default!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public DateOnly DateOnly { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(1000)]
        public string? Bio { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [Required]
      
        public IFormFile CvFile { get; set; }
        public enSkillLevel? SkillLevel { get; set; } = enSkillLevel.Beginner;


    }
}