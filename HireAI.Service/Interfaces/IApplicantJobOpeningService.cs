using HireAI.Data.DTOs;
using HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response.HireAI.Data.Helpers.DTOs.JopOpeningDtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IApplicantJobPostService
    {
        Task<List<JobPostResponseDto>> GetAllJobPostAsync();
    }
}
