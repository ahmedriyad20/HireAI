using AutoMapper;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenericBase;
using HireAI.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Implementation
{
    public class ExamService : IExamService
    {

        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;
        public ExamService(IExamRepository examRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _mapper = mapper;
        }

        public async Task<ExamDTO?> GetExamByApplicantIdAsync(int applicantId)
        {
            var exam = await _examRepository.GetExamByApplicanIdAsync(applicantId);
            if (exam == null) return null;
            var examDTO = _mapper.Map<ExamDTO>(exam);
            Console.WriteLine(examDTO.ExamName);

            return examDTO;
        }

        public async Task<ICollection<ExamDTO>> GetExamsTakenByApplicant(int aplicantId ,int pageNumber =1 , int pageSize=5)
        {
            var exams = await _examRepository.GetExamsByApplicantIdAsync(aplicantId, pageNumber ,pageSize) ?? new List<Exam>();
        
            return   exams.Select(e => _mapper.Map<ExamDTO>(e)).ToList();

        }

    }
}