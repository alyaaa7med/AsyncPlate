using AsyncPlate.Application.DTOs;
using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ReportService> _logger;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper , ILogger<ReportService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<DailyReportDTO> GenerateDailyReport()
        {
            var products = await _unitOfWork.products.GetTopSellingProductsAsync();

            var inventories = await _unitOfWork.inventories.GetLowStockInventory().ToListAsync();


            var report = new DailyReportDTO();

            report.TotalOrders =
                await _unitOfWork.orders.GetTodayOrdersCountAsync();

            report.CompletedOrders =
                await _unitOfWork.orders.GetCompletedOrdersCountAsync();

            report.CancelledOrders =
                await _unitOfWork.orders.GetCancelledOrdersCountAsync();

            report.TotalRevenue =
                await _unitOfWork.orders.GetTodayRevenueAsync();


            report.TopProducts = _mapper.Map<List<ProductResponseDTO>>(products);

            report.LowStockItems = _mapper.Map<List<InventoryResponseDTO>>(inventories);

            _logger.LogInformation("Daily report generated successfully at {Time}", DateTime.UtcNow);
            return report;
        }
    }
}
