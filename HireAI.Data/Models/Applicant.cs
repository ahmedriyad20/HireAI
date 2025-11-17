using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Models
{
    public class Applicant: User
    {
        public int CvId { get; set; }

        public int ApplicationId { get; set; }

        public int ApplicantSkillId { get; set; }

        public int TestId { get; set; }

        public string ResumeUrl { get; set; }

        //Navigation Property
        public virtual ICollection<ApplicantSkill> ApplicantSkills { get; set; } = new HashSet<ApplicantSkill>();
        public virtual ICollection<Application> Applications { get; set; } = new HashSet<Application>();
        public virtual ICollection<Exam> Exams { get; set; } = new HashSet<Exam>();
        public virtual CV? ApplicantCV { get; set; }

    }
}
