using HireAI.Data.Helpers.DTOs.Applicant;
using HireAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IApplicantService
    {
        Task<IEnumerable<ApplicantResponseDto>> GetAllApplicantsAsync();
        Task<ApplicantResponseDto?> GetApplicantByIdAsync(int applicantId);
        Task<ApplicantResponseDto> AddApplicantAsync(Applicant applicant);
        Task<ApplicantResponseDto> UpdateApplicantAsync(Applicant applicant);
        Task DeleteApplicantAsync(int applicantId);
    }
}
