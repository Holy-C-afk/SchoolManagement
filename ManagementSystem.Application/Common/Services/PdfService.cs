using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ManagementSystem.Application.Students.DTOs;
using ManagementSystem.Application.Teachers.DTOs;
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

                    page.Content().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(35); 
                            columns.RelativeColumn(3);  
                            columns.RelativeColumn(2);  
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(Block).Text("N°").Bold();
                            header.Cell().Element(Block).Text("Nom Complet").Bold();
                            header.Cell().Element(Block).Text("Date de Naissance").Bold();

                            static IContainer Block(IContainer container) => 
                                container.Border(1).BorderColor(Colors.Black).Padding(5).AlignCenter().Background(Colors.Grey.Lighten4);
                        });

                        int i = 1;
                        foreach (var student in students)
                        {
                            table.Cell().Element(Cell).Text(i++.ToString());
                            table.Cell().Element(Cell).Text(student.fullName);
                            table.Cell().Element(Cell).Text(student.dateOfBirth.ToString("dd/MM/yyyy"));

                            static IContainer Cell(IContainer container) => 
                                container.Border(1).BorderColor(Colors.Black).Padding(5);
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
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

        public byte[] GenerateTeachersPdf(IEnumerable<TeacherDto> teachers)
        {
            return Document.Create(container =>
            {
                // CORRECTION ICI : Ajout du bloc container.Page(page => ...)
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Header()
                        .Text("Liste des enseignants")
                        .SemiBold().FontSize(20).FontColor(Colors.Green.Medium);

                    page.Content().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(c => 
                        { 
                            c.ConstantColumn(30); 
                            c.RelativeColumn(3); 
                            c.RelativeColumn(2); 
                        });

                        table.Header(h => 
                        {
                            h.Cell().Element(Box).Text("#").Bold();
                            h.Cell().Element(Box).Text("NOM").Bold();
                            h.Cell().Element(Box).Text("DÉPARTEMENT").Bold();
                        });

                        int i = 1;
                        foreach (var t in teachers) 
                        {
                            table.Cell().Element(Box).Text(i++.ToString());
                            table.Cell().Element(Box).Text(t.FullName);
                            table.Cell().Element(Box).Text(t.DepartmentName);
                        }
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Page ");
                        text.CurrentPageNumber();
                    });
                });
            }).GeneratePdf();
        }

        // Méthode helper pour les bordures
        static IContainer Box(IContainer c) => c.Border(1).BorderColor(Colors.Black).Padding(5);
    }
}