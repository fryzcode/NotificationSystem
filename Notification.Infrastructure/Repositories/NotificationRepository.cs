using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Notification.Domain.Entities;
using Notification.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Notification.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
    private readonly string connectionString;

    public NotificationRepository(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task AddAsync(NotificationEntity notification, CancellationToken cancellationToken = default)
    {
        using IDbConnection db = new SqlConnection(connectionString);

        var sql = @"
            INSERT INTO Notifications (Id, Recipient, Message, Status)
            VALUES (@Id, @Recipient, @Message, @Status)";

        await db.ExecuteAsync(sql, new
        {
            Id = notification.Id,
            Recipient = notification.Recipient,
            Message = notification.Message,
            Status = notification.Status.ToString()
        });
    }
}
