using HireAI.Data.Helpers.DTOs.Exam.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IExamService
    {
        public Task<ExamResponseDTO?> GetExamByIdAsync(int examId);
        public Task<ExamResponseDTO?> GetExamByApplicantIdAsync(int applicantId);
        public Task<ICollection<ExamResponseDTO>> GetExamsTakenByApplicant(int aplicantId, int pageNumber = 1, int pageSize = 5)  ;

        public Task CreateExamAsync(ExamRequestDTO examRequesDTO);

        public Task CreateQuestionAsync(QuestionRequestDTO questionRequest);

        public Task DeleteExamAsync(int examId);

        /// <summary>
        /// Creates a job exam with AI-generated questions based on the job description from the application
        /// </summary>
        public Task<List<QuestionResponseDTO>> CreateJobExamAsync(int applicationId);

        /// <summary>
        /// Creates a mock exam with AI-generated questions based on the exam description
        /// </summary>
        public Task<List<QuestionResponseDTO>> CreateMockExamAsync(int examId);

        /// <summary>
        /// Evaluates an exam by creating ExamSummary and ExamEvaluation records
        /// </summary>
        public Task<ExamSummary> EvaluateJobExamAsync(ExamEvaluationRequestDTO evaluationRequest);

        public Task<ExamSummary> EvaluateMockExamAsync(ExamEvaluationRequestDTO evaluationRequest);
    }
}
