using HireAI.Data.Helpers.Enums;
using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Authentication
{
    public class RegisterHrDto
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

        [StringLength(200)]
        public string? HrAddress { get; set; }

        [Required(ErrorMessage = "Company Name is required")]
        [StringLength(200)]
        public string CompanyName { get; set; } = default!;

        [StringLength(100)]
        public string? CompanyAddress { get; set; }

        [StringLength(100)]
        public string? CompanyDescription { get; set; }

        [StringLength(100)]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateOnly DateOfBirth { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }

        public enAccountType? AccountType { get; set; } = enAccountType.Free;
    }
}