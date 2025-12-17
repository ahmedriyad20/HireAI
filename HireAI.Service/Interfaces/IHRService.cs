using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Data.Helpers.DTOs.Respones;
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
        Task<HRResponseDto> GetHRAsync(int hrId);
        public Task DeleteHRAsync(int hrId);
        public Task<HRResponseDto> CreateHRAsync(HRCreateDto hrCreateDto);
        public  Task<HRResponseDto> UpdateHRAsync(int hrId, HRUpdateDto hrUpdateDto);
        public Task<ICollection<HRResponseDto>> GetAllHRAsync();


    }
}