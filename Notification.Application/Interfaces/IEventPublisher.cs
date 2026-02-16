using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notification.Domain.Abstractions;

namespace Notification.Application.Interfaces;
public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
