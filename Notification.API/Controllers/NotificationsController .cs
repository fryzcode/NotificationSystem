using Microsoft.AspNetCore.Mvc;
using Notification.Application.Commands.CreateNotification;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly CreateNotificationHandler _handler;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(CreateNotificationHandler handler, ILogger<NotificationsController> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateNotificationCommand command,
            CancellationToken cancellationToken
            )
        {
            var id = await _handler.Handle(command, cancellationToken);
            return Ok(id);
        }
    }
}
