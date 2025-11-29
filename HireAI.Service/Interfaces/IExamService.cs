using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
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
        public Task<ExamResponseDTO?> GetExamByApplicantIdAsync(int applicantId);
        public Task<ICollection<ExamResponseDTO>> GetExamsTakenByApplicant(int aplicantId, int pageNumber = 1, int pageSize = 5)  ;

        public Task CreateExamAsync(ExamRequestDTO examRequesDTO);

        public Task CreateQuestionAsync(QuestionRequestDTO questionRequest);

        public Task DeleteExamAsync(int examId);

    }
}
