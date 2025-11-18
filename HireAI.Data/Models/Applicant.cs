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

        //Foreign Keys
        public int CvId { get; set; }

        //Navigation Property
        public virtual ICollection<ApplicantSkill> ApplicantSkills { get; set; } = new HashSet<ApplicantSkill>();
        public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public virtual ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public virtual ICollection<ExamSummary> ExamAttempts { get; set; } = new HashSet<ExamSummary>();
        public virtual CV? ApplicantCV { get; set; }

    }
}
