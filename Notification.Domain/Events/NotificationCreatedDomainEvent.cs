using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Domain.Abstractions;

namespace Notification.Domain.Events
{
    public class NotificationCreatedDomainEvent : IDomainEvent
    {
        public Guid NotificationId { get; }
        public string Recipient { get; }
        public string Message { get; }
        public DateTime OccurredOn { get; }
        public NotificationCreatedDomainEvent(Guid notificationId, string recipient, string message)
        {
            NotificationId = notificationId;
            Recipient = recipient;
            Message = message;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
