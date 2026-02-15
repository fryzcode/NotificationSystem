using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notification.Application.Commands.CreateNotification;
public record CreateNotificationCommand(string Recipient, string Message);

