using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Notification.Domain.Events;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using Dapper;
using Notification.Application.Interfaces;

namespace Notification.Worker.Services;



public class RabbitMqNotificationConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private IConnection _connection;
    private IModel _channel;

    public RabbitMqNotificationConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare("notification.exchange", ExchangeType.Fanout);

        _channel.QueueDeclare(
            queue: "notification.queue",
            durable: true,
            exclusive: false,
            autoDelete: false);

        _channel.QueueBind(
            queue: "notification.queue",
            exchange: "notification.exchange",
            routingKey: "");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());

                var domainEvent = JsonSerializer.Deserialize<NotificationCreatedDomainEvent>(json);

                if (domainEvent is null)
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                    return;
                }

                await ProcessNotification(domainEvent);

                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(
            queue: "notification.queue",
            autoAck: false,
            consumer: consumer);

        return Task.CompletedTask;
    }

    private async Task ProcessNotification(NotificationCreatedDomainEvent domainEvent)
    {
        using var scope = _serviceProvider.CreateScope();

        var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        await emailSender.SendAsync(
            domainEvent.Recipient,
              domainEvent.Subject,
            domainEvent.Message);

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        using IDbConnection db = new SqlConnection(connectionString);

        var sql = @"
        UPDATE Notifications
        SET Status = @Status
        WHERE Id = @Id";

        await db.ExecuteAsync(sql, new
        {
            Id = domainEvent.NotificationId,
            Status = "Sent"
        });
    }
}
