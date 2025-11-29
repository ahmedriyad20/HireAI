using HireAI.Data.Helpers.DTOs.ExamResponseDTOS.Request;
using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.ExamDTOS.Request
{

    public class QuestionRequestDTO
    {
        [Required(ErrorMessage = "Question text is required.")]
        [StringLength(1000, MinimumLength = 1, ErrorMessage = "Question text must be between 1 and 1000 characters.")]
        public string QuestionText { get; set; } = default!;

        public enQuestionAnswers? Answer { get; set; }

        [Range(1, 40, ErrorMessage = "Question number must be at least 1.")]
        public int QuestionNumber { get; set; }

        [MinLength(1, ErrorMessage = "Each question must have at least one answer option.")]
        public List<AnswerRequestDTO> Answers { get; set; } = new List<AnswerRequestDTO>();
    }
}
