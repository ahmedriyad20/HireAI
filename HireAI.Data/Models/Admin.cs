//using HireAI.Data.Helpers.Enums;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace HireAI.Data.Models
//{
//    [Table("Admins")]
//    public class Admin
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required(ErrorMessage = "Full name is required")]
//        [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters")]
//        public string FullName { get; set; } = default!;

//        [Required(ErrorMessage = "Email is required")]
//        [EmailAddress(ErrorMessage = "Invalid email address")]
//        [StringLength(256)]
//        public string Email { get; set; } = default!;

//        [Required(ErrorMessage = "Address is required")]
//        [StringLength(500)]
//        public string Address { get; set; } = default!;

//        [Required(ErrorMessage = "Date of birth is required")]
//        public DateOnly DateOfBirth { get; set; }

//        [Required]
//        public enRole Role { get; set; } = enRole.Admin;

//        public DateTime? LastLogin { get; set; }

//        [Required]
//        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
//    }
//}
