using AutoMapper;
using HireAI.Data.Helpers.DTOs.Exam.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Request;
using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using HireAI.Data.Helpers.Enums;
using HireAI.Data.Models;
using HireAI.Infrastructure.GenaricBasies;
using HireAI.Infrastructure.GenericBase;
using HireAI.Infrastructure.Intrefaces;
using HireAI.Infrastructure.Repositories;
using HireAI.Service.Interfaces;


namespace HireAI.Service.Services
{
    public class ExamService : IExamService
    {

        private readonly IExamRepository _examRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobPostRepository _jobPostRepository;
        private readonly IGeminiService _geminiService;
        private readonly IExamSummaryRepository _examSummaryRepository;
        private readonly IExamEvaluationRepository _examEvaluationRepository;
        private readonly IMapper _mapper;

        public ExamService(IExamRepository examRepository, IQuestionRepository questionRepository, IApplicationRepository applicationRepository,
            IJobPostRepository jobPostRepository, IGeminiService geminiService, IExamSummaryRepository examSummaryRepository,
            IExamEvaluationRepository examEvaluationRepository, IMapper mapper)
        {
            _examRepository = examRepository;
            _questionRepository = questionRepository;
            _applicationRepository = applicationRepository;
            _jobPostRepository = jobPostRepository;
            _geminiService = geminiService;
            _examSummaryRepository = examSummaryRepository;
            _examEvaluationRepository = examEvaluationRepository;
            _mapper = mapper;
        }

        public async Task<ExamResponseDTO?> GetExamByIdAsync(int examId)
        {
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null) return null;

            return _mapper.Map<ExamResponseDTO>(exam);
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

        public async Task<List<QuestionResponseDTO>> CreateJobExamAsync(int applicationId)
        {
            // Get application
            var application = await _applicationRepository.GetByIdAsync(applicationId);
            if (application == null)
                throw new Exception("Application not found");

            // Get job post
            if (application.JobId == null)
                throw new Exception("Application does not have an associated job");

            var jobPost = await _jobPostRepository.GetByIdAsync(application.JobId.Value);
            if (jobPost == null)
                throw new Exception("Job post not found");

            // Check if application already has an exam assigned
            if (application.ExamId != null)
            {
                // Return existing exam questions
                var existingExam = await _examRepository.GetByIdAsync(application.ExamId.Value);
                if (existingExam != null && existingExam.Questions.Any())
                {
                    return existingExam.Questions.Select(q => new QuestionResponseDTO
                    {
                        Id = q.Id,
                        QuestionText = q.QuestionText,
                        CorrectAnswerIndex = q.CorrectAnswerIndex,
                        AnswerChoices = q.Choices,
                        QuestionNumber = q.QuestionNumber,
                        ExamId = existingExam.Id
                    }).ToList();
                }
            }

            // Check if an exam already exists for this job post (from other applications)
            var existingJobExam = await _examRepository.GetExamByJobIdAsync(jobPost.Id);
            if (existingJobExam != null && existingJobExam.Questions.Any())
            {
                // Assign existing exam to this application
                application.ExamId = existingJobExam.Id;
                await _applicationRepository.UpdateAsync(application);

                return existingJobExam.Questions.Select(q => new QuestionResponseDTO
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    CorrectAnswerIndex = q.CorrectAnswerIndex,
                    AnswerChoices = q.Choices,
                    QuestionNumber = q.QuestionNumber,
                    ExamId = existingJobExam.Id
                }).ToList();
            }

            if (string.IsNullOrWhiteSpace(jobPost.Description))
                throw new Exception("Job post does not have a description");

            // Generate questions using Gemini AI (only if no exam exists)
            var aiQuestions = await _geminiService.GenerateJobExamQuestionsAsync(jobPost.Description);

            // Create exam entity
            var exam = new Exam
            {
                ExamName = $"Technical Exam for {jobPost.Title}",
                ExamDescription = $"AI-generated technical assessment for {jobPost.Title} position",
                ExamLevel = jobPost.ExperienceLevel switch
                {
                    enExperienceLevel.EntryLevel => enExamLevel.Beginner,
                    enExperienceLevel.Junior => enExamLevel.Beginner,
                    enExperienceLevel.MidLevel => enExamLevel.Intermediate,
                    enExperienceLevel.Senior => enExamLevel.Advanced,
                    enExperienceLevel.TeamLead => enExamLevel.Advanced,
                    enExperienceLevel.Executive => enExamLevel.Advanced,
                    _ => enExamLevel.Intermediate
                },
                NumberOfQuestions = aiQuestions.Questions.Count,
                DurationInMinutes = jobPost.ExamDurationMinutes ?? 15,
                CreatedAt = DateTime.Now,
                IsAi = true,
                ExamType = enExamType.HrExam
            };

            // Save exam to database
            await _examRepository.AddAsync(exam);

            // Create questions from AI response
            var questionResponses = new List<QuestionResponseDTO>();
            int questionNumber = 1;

            foreach (var aiQuestion in aiQuestions.Questions)
            {
                var question = new Question
                {
                    QuestionText = aiQuestion.QuestionText,
                    Choices = aiQuestion.Choices.ToArray(),
                    QuestionNumber = questionNumber,
                    CorrectAnswerIndex = aiQuestion.CorrectAnswerIndex,
                    ExamId = exam.Id
                };

                await _questionRepository.AddAsync(question);

                questionResponses.Add(new QuestionResponseDTO
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    CorrectAnswerIndex = question.CorrectAnswerIndex,
                    AnswerChoices = question.Choices,
                    QuestionNumber = question.QuestionNumber,
                    ExamId = exam.Id
                });

                questionNumber++;
            }

            // Update application with exam
            application.ExamId = exam.Id;
            await _applicationRepository.UpdateAsync(application);

            return questionResponses;
        }

        public async Task<List<QuestionResponseDTO>> CreateMockExamAsync(int examId)
        {
            // Get exam with questions
            var exam = await _examRepository.GetByIdAsync(examId);
            if (exam == null)
                throw new Exception("Exam not found");

            // Check if questions already exist for this exam
            if (exam.Questions != null && exam.Questions.Any())
            {
                // Return existing questions
                return exam.Questions.Select(q => new QuestionResponseDTO
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    CorrectAnswerIndex = q.CorrectAnswerIndex,
                    AnswerChoices = q.Choices,
                    QuestionNumber = q.QuestionNumber,
                    ExamId = exam.Id
                }).ToList();
            }

            if (string.IsNullOrWhiteSpace(exam.ExamDescription))
                throw new Exception("Exam does not have a description");

            // Generate questions using Gemini AI (only if no questions exist)
            var aiQuestions = await _geminiService.GenerateMockExamQuestionsAsync(exam.ExamDescription);

            // Create questions from AI response
            var questionResponses = new List<QuestionResponseDTO>();
            int questionNumber = 1;

            foreach (var aiQuestion in aiQuestions.Questions)
            {
                var question = new Question
                {
                    QuestionText = aiQuestion.QuestionText,
                    Choices = aiQuestion.Choices.ToArray(),
                    QuestionNumber = questionNumber,
                    CorrectAnswerIndex = aiQuestion.CorrectAnswerIndex,
                    ExamId = exam.Id
                };

                await _questionRepository.AddAsync(question);

                questionResponses.Add(new QuestionResponseDTO
                {
                    Id = question.Id,
                    QuestionText = question.QuestionText,
                    CorrectAnswerIndex = question.CorrectAnswerIndex,
                    AnswerChoices = question.Choices,
                    QuestionNumber = question.QuestionNumber,
                    ExamId = exam.Id
                });

                questionNumber++;
            }

            // Update exam question count
            exam.NumberOfQuestions = aiQuestions.Questions.Count;
            await _examRepository.UpdateAsync(exam);

            return questionResponses;
        }

        public async Task<ExamSummary> EvaluateJobExamAsync(ExamEvaluationRequestDTO evaluationRequest)
        {
            // Validate application exists
            var application = await _applicationRepository.GetByIdAsync(evaluationRequest.ApplicationId);
            if (application == null)
                throw new Exception("Application not found");

            // Validate exam exists
            var exam = await _examRepository.GetByIdAsync(evaluationRequest.ExamId);
            if (exam == null)
                throw new Exception("Exam not found");

            // Validate job post exists
            var jobPost = await _jobPostRepository.GetByIdAsync(evaluationRequest.JobId);
            if (jobPost == null)
                throw new Exception("Job post not found");

            // Create ExamSummary entry
            var examSummary = new ExamSummary
            {
                ApplicationId = evaluationRequest.ApplicationId,
                ExamId = evaluationRequest.ExamId,
                ApplicantExamScore = evaluationRequest.ApplicantExamScore,
                AppliedAt = evaluationRequest.AppliedAt ?? DateTime.Now,
                ApplicantId = application.ApplicantId,
                ExamEvaluationId = null // Will be set after creating ExamEvaluation
            };

            await _examSummaryRepository.AddAsync(examSummary);

            enExamEvaluationStatus examEvaluationStatus = evaluationRequest.ApplicantExamScore >= 80f?
                enExamEvaluationStatus.Passed : enExamEvaluationStatus.Failed;
            // Create ExamEvaluation entry
            var examEvaluation = new ExamEvaluation
            {
                ExamSummaryId = examSummary.Id,
                JobId = evaluationRequest.JobId,
                ApplicantExamScore = evaluationRequest.ApplicantExamScore,
                ExamTotalScore = evaluationRequest.ExamTotalScore ?? 100,
                EvaluatedAt = DateTime.Now,
                Status = examEvaluationStatus,
                QuestionEvaluations = new List<QuestionEvaluation>()
            };

            await _examEvaluationRepository.AddAsync(examEvaluation);

            //Update application status based on exam evaluation
            application.ExamStatus = enExamStatus.Completed;
            application.ApplicationStatus = enApplicationStatus.Completed;
            application.ExamEvaluationId = examEvaluation.Id;
            if(application.ExamId == null)
            {
                application.ExamId = exam.Id;
            }
            await _applicationRepository.UpdateAsync(application);

            // Update ExamSummary with ExamEvaluationId
            examSummary.ExamEvaluationId = examEvaluation.Id;
            await _examSummaryRepository.UpdateAsync(examSummary);

            return examSummary;
        }

        public async Task<ExamSummary> EvaluateMockExamAsync(ExamEvaluationRequestDTO evaluationRequest)
        {
            // Validate exam exists
            var exam = await _examRepository.GetByIdAsync(evaluationRequest.ExamId);
            if (exam == null)
                throw new Exception("Exam not found");

            // Create ExamSummary entry
            var examSummary = new ExamSummary
            {
                ExamId = evaluationRequest.ExamId,
                ApplicantExamScore = evaluationRequest.ApplicantExamScore,
                AppliedAt = evaluationRequest.AppliedAt ?? DateTime.Now,
                ApplicantId = evaluationRequest.ApplicantId,
                ExamEvaluationId = null // Will be set after creating ExamEvaluation
            };

            await _examSummaryRepository.AddAsync(examSummary);

            enExamEvaluationStatus examEvaluationStatus = evaluationRequest.ApplicantExamScore >= 80f ?
                enExamEvaluationStatus.Passed : enExamEvaluationStatus.Failed;
            // Create ExamEvaluation entry
            var examEvaluation = new ExamEvaluation
            {
                ExamSummaryId = examSummary.Id,
                ApplicantExamScore = evaluationRequest.ApplicantExamScore,
                ExamTotalScore = evaluationRequest.ExamTotalScore ?? 100,
                EvaluatedAt = DateTime.Now,
                Status = examEvaluationStatus,
                QuestionEvaluations = new List<QuestionEvaluation>()
            };

            await _examEvaluationRepository.AddAsync(examEvaluation);

            // Update ExamSummary with ExamEvaluationId
            examSummary.ExamEvaluationId = examEvaluation.Id;
            await _examSummaryRepository.UpdateAsync(examSummary);

            return examSummary;
        }

    }
}