
using HireAI.Data.Helpers.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Respones
{
    public class RecentApplicationDto
    {
          public String ApplicantName { get; set; }
          public string Position { get; set; } = string.Empty;
          //for sort on the front end
          public DateTime AppliedOn { get; set; }
          public float ATSScore { get; set; }

         public enJobStatus JobStatus { get; set; }

         public string ExamResultLink { get; set; }

        public string ApplicantCVlink { get; set; } 
    }
}
