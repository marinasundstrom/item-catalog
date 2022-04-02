using System.Net.Mail;
using System.Text;

namespace Catalog.Notifications.Services;

public class EmailService : IEmailService
{
    private const string sender = "noreply@email.com";

    public async Task Send(string emailAddress, string subject, string content)
    {
        var message = new MailMessage(sender, emailAddress);

        message.Subject = subject;
        message.Body = content;
        message.IsBodyHtml = true;
        message.BodyEncoding = Encoding.UTF8;
        //message.Attachments.Add(new Attachment(File.OpenRead("example.pdf"), "example.pdf"));

        SmtpClient smtpClient = new SmtpClient("localhost", 25);
        await smtpClient.SendMailAsync(message);
    }
}