using HireAI.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IApplicantJobOpeningService
    {
        public Task<List<JobOpeningDTO>> GetAllJobOpeningAsync();
    }
}
