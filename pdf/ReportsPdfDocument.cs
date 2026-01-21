using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using UserRoles.Models;

namespace UserRoles.Pdf
{
    public class ReportsPdfDocument : IDocument
    {
        private readonly List<DailyReport> _reports;
        private readonly string _userName;

        public ReportsPdfDocument(List<DailyReport> reports, string userName)
        {
            _reports = reports;
            _userName = userName;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);

                page.Header().Element(ComposeHeader);
                page.Content().Element(ComposeContent);
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Generated on ");
                    x.Span(DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                });
            });
        }

        void ComposeHeader(IContainer container)
        {
            container.Column(col =>
            {
                col.Item().Text("Employee Reports")
                    .FontSize(18)
                    .Bold();

                col.Item().Text($"User: {_userName}")
                    .FontSize(12)
                    .Italic();

                col.Item().PaddingVertical(5).LineHorizontal(1);
            });
        }

        void ComposeContent(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn(2); // Date
                    columns.RelativeColumn(3); // Task
                    columns.RelativeColumn(4); // Note
                    columns.RelativeColumn(2); // Reported To
                });

                table.Header(header =>
                {
                    header.Cell().Text("Date").Bold();
                    header.Cell().Text("Task").Bold();
                    header.Cell().Text("Note").Bold();
                    header.Cell().Text("Reported To").Bold();
                });

                foreach (var r in _reports)
                {
                    table.Cell().Text(r.Date.ToString("dd-MM-yyyy"));
                    table.Cell().Text(r.Task);
                    table.Cell().Text(r.Note);
                    table.Cell().Text(r.ReportedTo);
                }
            });
        }
    }
}
