using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.ExamDTOS.Request
{
        public class ExamRequestDTO
        {
            [Required(ErrorMessage = "Exam name is required.")]
            [StringLength(200, MinimumLength = 2, ErrorMessage = "Exam name must be between 2 and 200 characters.")]
            public string ExamName { get; set; } = default!;

            [Range(1, 20, ErrorMessage = "Number of questions must be at least 1.")]
            public int NumberOfQuestions { get; set; }

            [Range(1, 40, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
            public int DurationInMinutes { get; set; }

            public bool IsAi { get; set; } = true;

            [EnumDataType(typeof(enExamType), ErrorMessage = "Invalid exam type.")]
            public enExamType ExamType { get; set; } = enExamType.MockExam;

            // Optional associations (set by client or left null for server to assign)
            public int? ApplicantId { get; set; }
            public int? ApplicationId { get; set; }

            [MinLength(1, ErrorMessage = "At least one question is required.")]
            public List<QuestionRequestDTO> Questions { get; set; } = new List<QuestionRequestDTO>();
        }


    }

