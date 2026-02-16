using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Domain.Abstractions;
using Notification.Domain.Enums;
using Notification.Domain.Events;

namespace Notification.Domain.Entities;
public class NotificationEntity
{
    public Guid Id { get; private set; }
    public string Recipient { get; private set; }
    public string Message { get; private set; }
    public NotificationStatus Status { get; private set; }
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public NotificationEntity(string recipient, string message)
    {
        Id = Guid.NewGuid();
        Recipient = recipient;
        Message = message;
        Status = NotificationStatus.Pending;

        AddDomainEvent(new NotificationCreatedDomainEvent(Id, recipient, message));
    }
    public void MarkAsSent()
    {
        if (Status == NotificationStatus.Sent)
            throw new InvalidOperationException("Already sent");

        Status = NotificationStatus.Sent;
    }

    public void MarkAsFailed()
    {
        Status = NotificationStatus.Failed;
    }
}
