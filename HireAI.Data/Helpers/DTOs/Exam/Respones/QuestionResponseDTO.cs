
using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.ExamDTOS.Respones
{
    public class QuestionResponseDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = default!;
        public enQuestionAnswers? Answer { get; set; }
        public int QuestionNumber { get; set; }

        public int ExamId { get; set; }
        public int? ApplicantResponseId { get; set; }

        public List<AnswerResponseDTO> Answers { get; set; } = new List<AnswerResponseDTO>();
    }
  
}
