using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IHRService
    {

        public Task<HRDashboardDto> GetDashboardAsync(int hrId);
        public Task<HRResponseDto> GetHRAsync(int hrId);
    }
}
