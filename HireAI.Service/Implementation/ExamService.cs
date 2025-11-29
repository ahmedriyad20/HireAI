using AutoMapper;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Models;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Service.Interfaces;


namespace HireAI.Service.Implementation
{
    public class ExamService : IExamService
    {

        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;   
        private readonly IMapper _mapper;
        public ExamService(IExamRepository examRepository,IQuestionRepository questionRepository,  IMapper mapper)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task CreateExamAsync(ExamRequestDTO examRequesDTO)
        {
            var exam = _mapper.Map<Exam>(examRequesDTO);
            await _examRepository.CreateExamAsncy(exam);
        }

        public Task CreateQuestionAsync(QuestionRequestDTO questionRequest)
        {
           var question = _mapper.Map<Question>(questionRequest);
            return _questionRepository.AddAsync(question);
        }

        public async Task DeleteExamAsync(int examId)
        {
            var exam =  await _examRepository.GetByIdAsync(examId);
            if (exam == null) throw new Exception("Exam not found");

            await _examRepository.DeleteAsync(exam);
        }

        public async Task<ExamResponseDTO?> GetExamByApplicantIdAsync(int applicantId)
        {
            var exam = await _examRepository.GetExamByApplicanIdAsync(applicantId);
            if (exam == null) return null;
            var examDTO = _mapper.Map<ExamResponseDTO>(exam);
            Console.WriteLine(examDTO.ExamName);

            return examDTO;
        }

        public async Task<ICollection<ExamResponseDTO>> GetExamsTakenByApplicant(int aplicantId ,int pageNumber =1 , int pageSize=5)
        {
            var exams = await _examRepository.GetExamsByApplicantIdAsync(aplicantId, pageNumber ,pageSize) ?? new List<Exam>();
        
            return   exams.Select(e => _mapper.Map<ExamResponseDTO>(e)).ToList();

        }

    }
}