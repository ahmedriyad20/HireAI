using System.ComponentModel.DataAnnotations;

namespace HireAI.Data.Helpers.DTOs.Authentication
{
    public class DeleteAccountDto
    {
        [Required(ErrorMessage = "Password is required to confirm account deletion")]
        public string Password { get; set; } = string.Empty;
    }
}