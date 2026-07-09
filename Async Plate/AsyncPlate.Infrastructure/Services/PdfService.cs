using AsyncPlate.Application.DTOs;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Domain.Entities;
using Microsoft.Extensions.Logging;
using QuestPDF.Drawing;
using QuestPDF.Elements;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
public class PdfService : IPdfService
{
    private readonly ILogger<PdfService> _logger;

    public PdfService(ILogger<PdfService> logger)
    {
        _logger = logger;
    }
    public string GenerateDailyReportPdf(DailyReportDTO report)
    {
        byte[] pdfBytes = Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Margin(30);

                page.Header().Text("AsyncPlate Daily Report")
                    .Bold()
                    .FontSize(24);

                page.Content().Column(column =>
                {
                    column.Spacing(20);

                    SummaryCards(column, report);

                    TopProductsTable(column, report);

                    InventoryTable(column, report);
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Generated on {DateTime.Now:dd MMM yyyy HH:mm}");
            });

        }).GeneratePdf();

        _logger.LogInformation("PDF generated.");

        return Convert.ToBase64String(pdfBytes);
    }
    private void SummaryCards(ColumnDescriptor column, DailyReportDTO report)
    {
        column.Item().Text("Summary")
            .Bold()
            .FontSize(18);

        column.Item().Row(row =>
        {
            row.RelativeItem().Element(x =>
                Card(x, "Orders", report.TotalOrders.ToString()));

            row.RelativeItem().PaddingHorizontal(5).Element(x =>
                Card(x, "Completed", report.CompletedOrders.ToString()));

            row.RelativeItem().PaddingHorizontal(5).Element(x =>
                Card(x, "Cancelled", report.CancelledOrders.ToString()));

            row.RelativeItem().Element(x =>
                Card(x, "Revenue", $"EGP {report.TotalRevenue:N2}"));
        });
    }
    private void Card(IContainer container, string title, string value)
    {
        container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(10)
            .Column(column =>
            {
                column.Item().Text(title)
                    .FontColor(Colors.Grey.Darken1);

                column.Item().Text(value)
                    .Bold()
                    .FontSize(20);
            });
    }
    private void TopProductsTable(ColumnDescriptor column, DailyReportDTO report)
    {
        column.Item().Text("Top Selling Products")
            .Bold()
            .FontSize(18);

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(4);
                columns.RelativeColumn(1);
            });

            table.Header(header =>
            {
                header.Cell().BorderBottom(1).Padding(5)
                    .Text("Product").Bold();

                header.Cell().BorderBottom(1).Padding(5)
                    .AlignRight()
                    .Text("Orders").Bold();
            });

            foreach (var product in report.TopProducts)
            {
                table.Cell()
                    .BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(5)
                    .Text(product.Name);

                table.Cell()
                    .BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(5)
                    .AlignRight()
                    .Text(product.TotalTimesOrdered.ToString());
            }
        });
    }
    private void InventoryTable(ColumnDescriptor column, DailyReportDTO report)
    {
        column.Item().Text("Low Stock Inventory")
            .Bold()
            .FontSize(18);

        column.Item().Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.RelativeColumn(3);
                columns.RelativeColumn();
                columns.RelativeColumn();
            });

            table.Header(header =>
            {
                header.Cell().BorderBottom(1).Padding(5)
                    .Text("Item").Bold();

                header.Cell().BorderBottom(1).Padding(5)
                    .AlignCenter()
                    .Text("Current").Bold();

                header.Cell().BorderBottom(1).Padding(5)
                    .AlignCenter()
                    .Text("Minimum").Bold();
            });

            foreach (var item in report.LowStockItems)
            {
                table.Cell()
                    .BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(5)
                    .Text(item.Name);

                table.Cell()
                    .BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(5)
                    .AlignCenter()
                    .Text(item.CurrentStock.ToString());

                table.Cell()
                    .BorderBottom(1)
                    .BorderColor(Colors.Grey.Lighten2)
                    .Padding(5)
                    .AlignCenter()
                    .Text(item.MinStockLevel.ToString());
            }
        });
    }
}
