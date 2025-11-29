using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  HireAI.Data.Helpers.DTOs.ExamDTOS.Respones
{
    public class AnswerResponseDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public bool IsCorrect { get; set; } = false;

        public int QuestionId { get; set; }
    }
}
