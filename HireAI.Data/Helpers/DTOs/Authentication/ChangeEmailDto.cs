using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Authentication
{
    public class ChangeEmailDto
    {
        [Required(ErrorMessage = "New email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string NewEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm email is required")]
        [Compare("NewEmail", ErrorMessage = "Email addresses do not match")]
        public string ConfirmNewEmail { get; set; } = string.Empty;
    }
}