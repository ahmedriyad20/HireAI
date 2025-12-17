using HireAI.Data.Helpers.DTOs.ReportDtos.resposnes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Interfaces
{
    public interface IReportPdfService
    {
        byte[] GeneratePdf(ReportDto report);
    }

}
