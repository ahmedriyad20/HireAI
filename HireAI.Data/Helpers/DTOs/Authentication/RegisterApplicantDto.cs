using HireAI.Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Authentication
{
    public class RegisterApplicantDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 4, ErrorMessage = "Password must be at least 4 characters")]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        //    ErrorMessage = "Password must contain uppercase, lowercase, number, and special character")]
        public string Password { get; set; } = default!;

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = default!;

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(200)]
        public string FullName { get; set; } = default!;

        [StringLength(100)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly DateOfBirth { get; set; }

        [StringLength(100)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Resume File PDF is required")]
        public IFormFile CvFile { get; set; }

        public enSkillLevel? SkillLevel { get; set; } = enSkillLevel.Beginner;
    }
}
