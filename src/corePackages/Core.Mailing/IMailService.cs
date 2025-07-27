namespace Core.Mailing;

public interface IMailService
{
    void SendMail(Mail mail);
    void SendMail(Mail mail, MailSettings? mailSettings);
    Task SendEmailAsync(Mail mail);
    Task SendEmailAsync(Mail mail, MailSettings? mailSettings);
}
