using AsyncPlate.Application.DTOs;
using AsyncPlate.Application.Interfaces.Services;

using System.Reflection.Metadata;
using System.Text;
using QuestPDF.Fluent;

namespace AsyncPlate.Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        public string GenerateDailyReportPdf(DailyReportDTO report)
        {
            byte[] pdfBytes = QuestPDF.Fluent.Document.Create(container =>
           {
               container.Page(page =>
               {
                   page.Margin(30);

                   page.Header()
                       .Text("AsyncPlate Daily Report")
                       .FontSize(24)
                       .Bold();

                   page.Content().Column(column =>
                   {
                       column.Spacing(10);

                       column.Item().Text($"Date: {DateTime.UtcNow:dd/MM/yyyy}");

                       column.Item().Text("Orders Summary").Bold();

                       column.Item().Text(
                           $"Total Orders: {report.TotalOrders}");

                       column.Item().Text(
                           $"Completed Orders: {report.CompletedOrders}");

                       column.Item().Text(
                           $"Cancelled Orders: {report.CancelledOrders}");

                       column.Item().Text(
                           $"Revenue: {report.TotalRevenue:C}");


                       column.Item().PaddingTop(20);

                       column.Item().Text("Top Products").Bold();

                       foreach (var product in report.TopProducts)
                       {
                           column.Item().Text(
                               $"{product.Name} - {product.TotalTimesOrdered}");
                       }

                       column.Item().PaddingTop(20);

                       column.Item().Text("Low Stock Items").Bold();

                       foreach (var item in report.LowStockItems)
                       {
                           column.Item().Text(
                               $"{item.Name} | Current: {item.CurrentStock} | Min: {item.MinStockLevel}");
                       }
                   });
               });
           }).GeneratePdf();

            string pdfBase64 = Convert.ToBase64String(pdfBytes);
            return pdfBase64;
        }
    }
}

