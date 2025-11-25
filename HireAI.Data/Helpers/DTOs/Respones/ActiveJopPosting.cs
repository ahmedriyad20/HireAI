using HireAI.Data.Helpers.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Data.Helpers.DTOs.Respones
{
    public class ActiveJopPosting
    {
        public string JobTitle { get; set; } = string.Empty;
        public int ApplicationTotalCount { get; set; }
        public enJobStatus JobStatus { get; set; }

        public int TakenExamCount { get; set; }

        public string JobPostLink { get; set; } = string.Empty; // to manage this Application

    }
}
