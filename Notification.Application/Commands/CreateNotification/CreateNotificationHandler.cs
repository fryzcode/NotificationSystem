using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Application.Commands.CreateNotification;

public class CreateNotificationHandler
{
    private readonly INotificationRepository _repository;

    public CreateNotificationHandler(INotificationRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateNotificationCommand command)
    {
        var notification = new NotificationEntity(command.Recipient, command.Message);

        await _repository.AddAsync(notification);

        return notification.Id;
    }
}
