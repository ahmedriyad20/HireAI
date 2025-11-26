using HireAI.Data.Helpers.DTOs.ExamDTOS.Respones;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IExamService
    {
        public Task<ExamDTO?> GetExamByApplicantIdAsync(int applicantId);
        public void GetExamsTakenByApplicant(int aplicantID);
    }
}
