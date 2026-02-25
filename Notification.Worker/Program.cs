using Notification.Application.Interfaces;
using Notification.Infrastructure.Email;
using Notification.Worker;
using Notification.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<RabbitMqNotificationConsumer>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Configuration.AddUserSecrets<Program>();

var host = builder.Build();
host.Run();
