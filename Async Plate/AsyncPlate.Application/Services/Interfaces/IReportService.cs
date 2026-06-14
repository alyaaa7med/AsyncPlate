using AsyncPlate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<DailyReportDTO> GenerateDailyReport();
    }
}
