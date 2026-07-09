using AsyncPlate.Application.DTOs;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;

namespace AsyncPlate.Infrastructure.Services.Jobs
{

    public class ReportJob : IReportJob
    {
        private readonly IReportService _reportService;
        private readonly IPdfService _pdfService;
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public ReportJob(
            IReportService reportService,
            IPdfService pdfService ,
            IEmailService emailService,
            IUnitOfWork unitOfWork)
        {
            _reportService = reportService;
            _pdfService = pdfService;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync()
        {
            var report =
                await _reportService.GenerateDailyReport();

            var pdf =
                _pdfService.GenerateDailyReportPdf(report);


            var emails = await _unitOfWork.admins.GetAdminsEmailsAsync();

            foreach (var adminemail in emails)
            {
                if(adminemail == null)
                {
                    continue;
                }

              await _emailService.SendEmailWithAttachmentAsync(
              adminemail,
              "Daily Restaurant Report",
              "Please find today's report attached.",
              pdf,
              $"DailyReport_{DateTime.UtcNow:yyyyMMdd}.pdf");
            }
          
        }
    }
}