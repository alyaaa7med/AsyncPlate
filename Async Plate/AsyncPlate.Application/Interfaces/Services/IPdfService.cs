using AsyncPlate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Services
{
    public interface  IPdfService
    {
        string GenerateDailyReportPdf(DailyReportDTO report);

    }
}
