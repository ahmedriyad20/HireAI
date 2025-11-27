
using HireAI.Data.Helpers.DTOs.JopOpening.Request;
using HireAI.Data.Helpers.DTOs.JopOpening.ResonsetDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IJopOpenningService
    {
        public Task AddJopOppenAsny(CreateJopOpeingRequestDto jopOpeingRequestDto);

        public Task CreateJopOppenAsny(JopOpeingRequestDto jopOpeingRequestDto);
        public Task DeleteJopOppenAsny(int id);

        public Task<ICollection<JobOpeningResponseDto>> GetJopOpeningForHrAsync(int hrid);
        public Task UpdateJopOppenAsny(int id, JopOpeingRequestDto jopOpeingRequestDto);

    }
}
