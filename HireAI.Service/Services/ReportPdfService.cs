using HireAI.Data.Helpers.DTOs.ReportDtos.resposnes;
using HireAI.Service.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace HireAI.Service.Services
{
    public class ReportPdfService : IReportPdfService
    {
        public byte[] GeneratePdf(ReportDto report)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text($"Job Report: {report.JobTitle}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Total Applicants: {report.TotalApplicants}");
                        col.Item().Text($"ATS Pass %: {report.AtsPassPercent:F1}%");
                        col.Item().Text("Status Counts:");
                        //foreach (var status in report.StatusCounts)
                        //    col.Item().Text($"  {status.Key}: {status.Value}");

                        col.Item().Text("Applicants:").Bold();
                        col.Item().Table(table =>
                        {
                            // Define columns
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(50); // Name
                                columns.RelativeColumn();   // Email
                                columns.ConstantColumn(50); // ATS Score
                                columns.ConstantColumn(50); // Exam Score
                                columns.ConstantColumn(60); // Status
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Name");
                                header.Cell().Element(CellStyle).Text("Email");
                                header.Cell().Element(CellStyle).Text("ATS");
                                header.Cell().Element(CellStyle).Text("Exam");
                                header.Cell().Element(CellStyle).Text("Status");
                            });

                            // Rows
                            foreach (var applicant in report.Applicants)
                            {
                                table.Cell().Element(CellStyle).Text(applicant.Name);
                                table.Cell().Element(CellStyle).Text(applicant.Email);
                                table.Cell().Element(CellStyle).Text(applicant.AtsScore.ToString());
                                table.Cell().Element(CellStyle).Text(applicant.ExamScore?.ToString() ?? "-");
                                table.Cell().Element(CellStyle).Text(applicant.Status);
                            }

                            static IContainer CellStyle(IContainer container) =>
                                container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(2);
                        });
                    });

                page.Footer()
                    .AlignCenter()
                    .Text(x => x.Span($"Generated on {DateTime.Now}"));
                });
            });

            return pdf.GeneratePdf();
        }
    }
}
