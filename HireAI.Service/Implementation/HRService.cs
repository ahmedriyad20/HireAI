using AutoMapper;
using HireAI.Data.Helpers.DTOs.HRDTOS;
using HireAI.Data.Helpers.DTOs.Respones;
using HireAI.Data.Helpers.DTOs.Respones.HRDashboardDto;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenericBase;
using HireAI.Service.Interfaces;
using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Implementation
{

    public class HRService : IHRService
    {
      
        private readonly IHRRepository _hr;
        private readonly IMapper _map;

        public HRService(IHRRepository hr , IMapper mapper)
        {
            _hr = hr;
            _map = mapper;
        }
        //Crud Operation for HR
        public async Task<HRResponseDto> GetHRAsync(int hrId)
        {
            var hr = await _hr.GetAll()
                .Where(h => h.Id == hrId)
                .FirstOrDefaultAsync();

            if (hr == null)
            {
                throw new Exception($"HR with ID {hrId} not found.");
            }

            var hrResponse = _map.Map<HRResponseDto>(hr);
            return hrResponse;
        }

  
        public async Task DeleteHRAsync(int hrId)
        {
            var hr = await _hr.GetByIdAsync(hrId);
            if (hr == null)
            {
                throw new Exception($"HR with ID {hrId} not found.");
            }
            await _hr.DeleteAsync(hr);
        }
        public async Task<HRResponseDto> UpdateHRAsync(int hrId, HRUpdateDto hrUpdateDto)
        {
            var hr = await _hr.GetByIdAsync(hrId);
            if (hr == null)
            {
                throw new Exception($"HR with ID {hrId} not found.");
            }
            _map.Map(hrUpdateDto, hr);
            await _hr.UpdateAsync(hr);
            var hrResponse = _map.Map<HRResponseDto>(hr);
            return hrResponse;
        }

        
        public async Task<HRResponseDto> CreateHRAsync(HRCreateDto hrCreateDto)
        {
            var hr = _map.Map<HR>(hrCreateDto);
            await _hr.AddAsync(hr);
            var hrResponse = _map.Map<HRResponseDto>(hr);
            return hrResponse;
        }

      
    }


}
