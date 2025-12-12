using HireAI.Data.Helpers.Enums;


namespace HireAI.Data.Models
{
    public  class Application
    {
        public int Id { get; set; }
        public enApplicationStatus ApplicationStatus { get; set; } = enApplicationStatus.ATSPassed;
        public DateTime DateApplied { get; set; } = DateTime.Now;
        public string? CVFilePath { get; set; }
        public float? AtsScore { get; set; }

        //Foreign Keys
        public int? HRId { get; set; }
        public int? ApplicantId { get; set; }
        public int? JobId { get; set; }
        public int? ExamId { get; set; }
        public int? ExamEvaluationId { get; set; }  

        //Navigation Property
        public HR? HR { get; set; }
        public Applicant? Applicant { get; set; }
        public JobPost? AppliedJob { get; set; }
        public Exam? Exam { get; set; }
        public ExamSummary? ExamSummary { get; set; }
        public ExamEvaluation? ExamEvaluation { get; set; }

        //add this exam status
        public enExamStatus ExamStatus { get; set; } = enExamStatus.NotTaken;


    }
}
