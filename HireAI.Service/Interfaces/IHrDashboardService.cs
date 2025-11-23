using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    interface IHrDashboardService
    {

        public Task<HRDashboardDto> GetDashboardAsync(int hrId);
    }
}
