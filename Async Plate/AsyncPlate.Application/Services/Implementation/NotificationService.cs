using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Exceptions;
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

        private readonly IRealtimeService _notificationSender;
        private readonly IOfferJob _offerJob;

        public NotificationService(ILogger<NotificationService> logger, IMapper mapper, IUnitOfWork unitOfWork
                                     , IRealtimeService notificationSender, IOfferJob offerJob)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

            _notificationSender = notificationSender;
            _offerJob = offerJob;
        }


        public async Task<PagedResult<NotificationResponseDTO>> GetAllNotificationsAsync( string userId,int pageNumber,int pageSize)
        {
            _logger.LogInformation(
                "Retrieving notifications for user {UserId}. Page: {PageNumber}, PageSize: {PageSize}",
                userId,pageNumber,pageSize);

            var query = _unitOfWork.notifications.GetAllByUserId(userId);

            var pagedResult = await query.ToPagedResultAsync(pageNumber, pageSize);


              

            return new PagedResult<NotificationResponseDTO>
            {
                Items = _mapper.Map<List<NotificationResponseDTO>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalPages= pagedResult.TotalPages
            };
        }


        public async Task<NotificationResponseDTO> GetNotificationByIdAsync(string userId, string notificationId)
        {
            _logger.LogInformation( "Retrieving notification {NotificationId} for user {UserId}.",
                    notificationId, userId);

            var notification = await _unitOfWork.notifications.GetByIdAsync(notificationId);

            if (notification == null)
            {
                _logger.LogWarning( "Notification {NotificationId} not found.",notificationId);

                throw new Exceptions.NotFoundException("Notification not found.");
            }

            if (notification.userId != userId)
            {
                _logger.LogWarning("User {UserId} attempted to access notification {NotificationId} that does not belong to them.",
                    userId,notificationId);

                throw new Exceptions.ForbiddenException("You are not allowed to access this notification.");
            }

            return _mapper.Map<NotificationResponseDTO>(notification);
        }

        public async Task<NotificationResponseDTO> MarkNotificationAsReadAsync(string userId, string notificationId)
        {
            _logger.LogInformation("Marking notification {NotificationId} as read for user {UserId}.",notificationId,userId);

            var notification = await _unitOfWork.notifications.GetByIdAsync(notificationId);

            if (notification == null)
            {
                _logger.LogWarning("Notification {NotificationId} not found.", notificationId);

                throw new Exceptions.NotFoundException("Notification not found.");
            }
            if (notification.userId != userId)
            {
                _logger.LogWarning("User {UserId} attempted to access notification {NotificationId} that does not belong to them.", userId, notificationId);

                throw new Exceptions.ForbiddenException("You are not allowed to access this notification.");
            }


            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("Notification {NotificationId} marked as read.", notificationId);
            return _mapper.Map<NotificationResponseDTO>(notification);
        }
    }
}
