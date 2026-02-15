using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Domain.Enums;

namespace Notification.Domain.Entities;
public class NotificationEntity
{
    public Guid Id { get; private set; }
    public string Recipient { get; private set; }
    public string Message { get; private set; }
    public NotificationStatus Status { get; private set; }

    private NotificationEntity() { }
    public NotificationEntity(string recipient, string message)
    {
        Id = Guid.NewGuid();
        Recipient = recipient;
        Message = message;
        Status = NotificationStatus.Pending;
    }
    public void MarkAsSent()
    {
        Status = NotificationStatus.Sent;
    }

    public void MarkAsFailed()
    {
        Status = NotificationStatus.Failed;
    }
}
