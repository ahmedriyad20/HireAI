using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Applicant: User
    {
        public string ResumeUrl { get; set; } = default!;
        public enSkillLevel? SkillLevel { get; set; } = enSkillLevel.Beginner;

        //Foreign Keys
        public int? CVId { get; set; }

        //Navigation Property
        public virtual ICollection<ApplicantSkill> ApplicantSkills { get; set; } = new HashSet<ApplicantSkill>();
        public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public virtual ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public virtual ICollection<ExamSummary> ExamSummarys { get; set; } = new HashSet<ExamSummary>();
        public CV? CV { get; set; }
    }
}
