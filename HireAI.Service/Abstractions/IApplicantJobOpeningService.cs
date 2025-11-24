using HireAI.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Abstractions
{
    public interface IApplicantJobOpeningService
    {
        public Task<List<JobOpeningDTO>> GetAllJobOpeningAsync();
    }
}
