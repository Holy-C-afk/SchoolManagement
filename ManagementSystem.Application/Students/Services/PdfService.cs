using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ManagementSystem.Application.Common.Interfaces;

namespace ManagementSystem.Application.Students
{
    public class PdfService : IPdfService
    {
        public byte[] GenerateStudentsPdf(IEnumerable<StudentDto> students)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Liste des étudiants")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Nom complet
                                columns.RelativeColumn(1); // Date de naissance
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Nom Complet").SemiBold();
                                header.Cell().Text("Date de Naissance").SemiBold();
                            });

                            foreach (var student in students)
                            {
                                table.Cell().Text(student.fullName);
                                table.Cell().Text(student.dateOfBirth.ToString("yyyy-MM-dd"));
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(text =>
                        {
                            text.Span("Page ");
                            text.CurrentPageNumber();
                            text.Span(" sur ");
                            text.TotalPages();
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}
