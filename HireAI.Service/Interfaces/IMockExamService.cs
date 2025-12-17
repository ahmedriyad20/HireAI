using HireAI.Data.Helpers.DTOs.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IMockExamService
    {
        Task<int> GetMockExamsTakenNumberPerApplicantAsync(int applicantId);
        Task<double> GetAverageExamsTakenScorePerApplicantAsync(int applicantId);
        Task<double> GetAverageExamsTakenScoreImprovementPerApplicantAsync(int applicantId);
        Task<IEnumerable<MockExamDto>> GetRecommendedMockExamsPerApplicantAsync(int applicantId);
        Task<IEnumerable<MockExamDto>> GetAllMockExamsPerApplicantAsync(int applicantId);
    }
}
