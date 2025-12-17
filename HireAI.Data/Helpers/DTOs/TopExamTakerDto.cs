using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs
{
    /// <summary>
    /// DTO for top exam takers (by exam score)
    /// </summary>
    public class TopExamTakerDto
    {
        public int ApplicationId { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantPhone { get; set; }
        public string CVKey { get; set; }
        public DateTime DateApplied { get; set; }
        public float ExamScore { get; set; }
        public string Rank { get; set; } // "1st", "2nd", "3rd", etc.
    }
}
