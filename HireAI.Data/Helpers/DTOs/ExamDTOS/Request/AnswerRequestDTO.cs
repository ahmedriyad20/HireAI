using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.ExamResponseDTOS.Request
{

    public class AnswerRequestDTO
    {
        [Required(ErrorMessage = "Answer text is required.")]
        [StringLength(500, ErrorMessage = "Answer text must not exceed 500 characters.")]
        public string Text { get; set; } = default!;

        public bool IsCorrect { get; set; } = false;
    }
}
