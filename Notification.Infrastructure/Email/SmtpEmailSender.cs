using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Notification.Application.Interfaces;
using MailKit.Net.Smtp;

namespace Notification.Infrastructure.Email;
public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendAsync(string to, string subject, string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration["Smtp:SenderEmail"]));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart("plain") { Text = body };

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(
            _configuration["Smtp:Server"],
            int.Parse(_configuration["Smtp:Port"]),
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _configuration["Smtp:User"],
            _configuration["Smtp:Password"]);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}
