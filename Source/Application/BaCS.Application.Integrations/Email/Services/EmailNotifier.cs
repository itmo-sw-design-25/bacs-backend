namespace BaCS.Application.Integrations.Email.Services;

using Abstractions.Integrations;
using Constants;
using Domain.Core.Entities;
using Domain.Core.Extensions;
using Helpers;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit.Text;
using Options;
using Scriban;

public class EmailNotifier(IOptionsSnapshot<EmailOptions> options, ILogger<EmailNotifier> logger) : IEmailNotifier
{
    private readonly EmailOptions _emailOptions = options.Value;

    public async Task SendReservationCreated(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(user.Email)) return;
        if (user.EnableEmailNotifications is false) return;

        var templateParams = new
        {
            Name = user.Name,
            ReservationDate = reservation.From.Date.ToString("dd.MM.yyyy"),
            From = reservation.From.ToString("t"),
            To = reservation.To.ToString("t"),
            WorkspaceName = location.Name,
            WorkspaceAddressLink = $"https://2gis.ru/search/{Uri.EscapeDataString(location.Address)}",
            WorkspaceAddress = location.Address,
            ResourceFloor = $"{resource.Floor} этаж",
            ResourceType = resource.Type.GetDisplayName(),
            ResourceName = resource.Name,
            AdminEmail = "help@bacs.space"
        };

        var isSuccess = await TrySendReservationEvent(
            user.Email,
            messageSubject: "Подтверждение бронирования",
            template: EmailTemplateConstants.ReservationCreated,
            templateParams,
            cancellationToken
        );

        if (!isSuccess) return;

        logger.LogInformation(
            "Email {EmailTemplate} has been sent to user {UserEmail}",
            nameof(SendReservationCreated),
            user.Email
        );
    }

    public async Task SendReservationUpdated(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(user.Email)) return;
        if (user.EnableEmailNotifications is false) return;

        var templateParams = new
        {
            Name = user.Name,
            ReservationDate = reservation.From.Date.ToString("dd.MM.yyyy"),
            From = reservation.From.ToString("t"),
            To = reservation.To.ToString("t"),
            WorkspaceName = location.Name,
            WorkspaceAddressLink = $"https://2gis.ru/search/{Uri.EscapeDataString(location.Address)}",
            WorkspaceAddress = location.Address,
            ResourceFloor = $"{resource.Floor} этаж",
            ResourceType = resource.Type.GetDisplayName(),
            ResourceName = resource.Name,
            AdminEmail = "help@bacs.space"
        };

        var isSuccess = await TrySendReservationEvent(
            user.Email,
            messageSubject: "Обновление бронирования",
            template: EmailTemplateConstants.ReservationUpdated,
            templateParams,
            cancellationToken
        );

        if (!isSuccess) return;

        logger.LogInformation(
            "Email {EmailTemplate} has been sent to user {UserEmail}",
            nameof(SendReservationUpdated),
            user.Email
        );
    }

    public async Task SendReservationCancelled(
        Reservation reservation,
        Location location,
        Resource resource,
        User user,
        CancellationToken cancellationToken
    )
    {
        if (string.IsNullOrWhiteSpace(user.Email)) return;
        if (user.EnableEmailNotifications is false) return;

        var templateParams = new
        {
            Name = user.Name,
            ReservationDate = reservation.From.Date.ToString("dd.MM.yyyy"),
            From = reservation.From.ToString("t"),
            To = reservation.To.ToString("t"),
            WorkspaceName = location.Name,
            WorkspaceAddressLink = $"https://2gis.ru/search/{Uri.EscapeDataString(location.Address)}",
            WorkspaceAddress = location.Address,
            ResourceFloor = $"{resource.Floor} этаж",
            ResourceType = resource.Type.GetDisplayName(),
            ResourceName = resource.Name,
            AdminEmail = "help@bacs.space"
        };

        var isSuccess = await TrySendReservationEvent(
            user.Email,
            messageSubject: "Отмена бронирования",
            template: EmailTemplateConstants.ReservationCancelled,
            templateParams,
            cancellationToken
        );

        if (!isSuccess) return;

        logger.LogInformation(
            "Email {EmailTemplate} has been sent to user {UserEmail}",
            nameof(SendReservationCancelled),
            user.Email
        );
    }

    private async Task<bool> TrySendReservationEvent(
        string userEmail,
        string messageSubject,
        string template,
        object templateParams,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailOptions.ServiceName, _emailOptions.Username));
            message.To.Add(MailboxAddress.Parse(userEmail));
            message.Subject = messageSubject;

            var emailTemplate = EmbeddedResourceReader.ReadAsString(template);
            var parsedTemplate = Template.Parse(emailTemplate);

            var html = await parsedTemplate.RenderAsync(templateParams, member => member.Name);

            message.Body = new TextPart(TextFormat.Html) { Text = html };

            using var smtp = new SmtpClient();
            smtp.ServerCertificateValidationCallback = (sender, _, _, _) => sender.Equals(_emailOptions.SmtpServer);

            await smtp.ConnectAsync(
                _emailOptions.SmtpServer,
                _emailOptions.Port,
                SecureSocketOptions.Auto,
                cancellationToken
            );

            await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password, cancellationToken);
            await smtp.SendAsync(message, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);

            return true;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            logger.LogError(ex, "Failed to send email notification");

            return false;
        }
    }
}
