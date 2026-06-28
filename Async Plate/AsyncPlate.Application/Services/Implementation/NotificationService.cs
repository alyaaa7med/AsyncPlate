using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        private readonly INotificationSender _notificationSender;
        private readonly IOfferJob _offerJob;

        public NotificationService(ILogger<NotificationService> logger, IMapper mapper, IUnitOfWork unitOfWork
                                     , INotificationSender notificationSender, IOfferJob offerJob)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            _notificationSender = notificationSender;
            _offerJob = offerJob;
        }


        public async Task<PagedResult<NotificationResponseDTO>> GetAllNotificationsAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<NotificationResponseDTO> GetNotificationByIdAsync(string userId, string notificationId)
        {
            throw new NotImplementedException();

        }

        public async Task<NotificationResponseDTO> MarkNotificationAsReadAsync(string userId, string notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
