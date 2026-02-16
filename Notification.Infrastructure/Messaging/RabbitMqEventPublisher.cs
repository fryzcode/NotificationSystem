using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Notification.Application.Interfaces;
using Notification.Domain.Abstractions;
using RabbitMQ.Client;

namespace Notification.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;   

    public RabbitMqEventPublisher()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: "notification.exchange",
            type: ExchangeType.Fanout);
    }

    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(domainEvent);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "notification.exchange",
            routingKey: "",
            basicProperties: null,
            body: body);

        return Task.CompletedTask;
    }
}