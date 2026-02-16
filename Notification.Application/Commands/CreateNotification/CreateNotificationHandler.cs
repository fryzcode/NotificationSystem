using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Notification.Application.Interfaces;
using Notification.Domain.Entities;

namespace Notification.Application.Commands.CreateNotification;

public class CreateNotificationHandler
{
    private readonly INotificationRepository _repository;
    private readonly IEventPublisher _publisher;

    public CreateNotificationHandler(
        INotificationRepository repository,
        IEventPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateNotificationCommand command, CancellationToken cancellationToken)
    {
        var notification = new NotificationEntity(command.Recipient, command.Message);

        await _repository.AddAsync(notification);

        foreach (var domainEvent in notification.DomainEvents)
        {
            await _publisher.PublishAsync(domainEvent, cancellationToken);
        }

        return notification.Id;
    }
}
