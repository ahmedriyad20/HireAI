using HireAI.Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Applicant.Request
{
    /// <summary>
    /// DTO used to update an existing Applicant.
    /// Contains fields that can be modified after creation.
    /// </summary>
    public class ApplicantUpdateDto
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; } = default!;

        //[Required(ErrorMessage = "Full Name is required")]
        [StringLength(200)]
        public string? FullName { get; set; } = default!;

        [StringLength(100)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly DateOfBirth { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [StringLength(500)]
        public string? Bio { get; set; }

        //[Required(ErrorMessage = "Resume File PDF is required")]
        public IFormFile? CvFile { get; set; }

        public enSkillLevel? SkillLevel { get; set; } = enSkillLevel.Beginner;
    }
}