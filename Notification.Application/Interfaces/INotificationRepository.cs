using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Domain.Entities;


namespace Notification.Application.Interfaces;

public interface INotificationRepository
{
    Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken = default);
}
