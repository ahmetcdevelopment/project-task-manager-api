using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Cryptography;
using Core.Mailing;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using MailKit.Security;
using System.Security.Authentication;

namespace Core.Mailing.MailKit;

public class MailKitMailService : IMailService
{
    private MailSettings _mailSettings;

    public MailKitMailService(MailSettings configuration)
    {
        _mailSettings = configuration;
    }

    public void SendMail(Mail mail)
    {

        if (mail.ToList == null || mail.ToList.Count < 1)
            return;
        emailPrepare(mail, email: out MimeMessage email, smtp: out SmtpClient smtp);
        smtp.Send(email);
        smtp.Disconnect(true);
        email.Dispose();
        smtp.Dispose();
    }
    public void SendMail(Mail mail, MailSettings? mailSettings)
    {
        if(mailSettings != null)
            _mailSettings = mailSettings;

        if (mail.ToList == null || mail.ToList.Count < 1)
            return;
        emailPrepare(mail, email: out MimeMessage email, smtp: out SmtpClient smtp);
        smtp.Send(email);
        smtp.Disconnect(true);
        email.Dispose();
        smtp.Dispose();
    }

    public async Task SendEmailAsync(Mail mail)
    {
        if (mail.ToList == null || mail.ToList.Count < 1)
            return;
        emailPrepare(mail, email: out MimeMessage email, smtp: out SmtpClient smtp);
        try
        {
        await smtp.SendAsync(email);

        }
        catch (Exception)
        {

            throw;
        }
        smtp.Disconnect(true);
        email.Dispose();
        smtp.Dispose();
    }
    public async Task SendEmailAsync(Mail mail, MailSettings? mailSettings)
    {
        if (mailSettings != null)
            _mailSettings = mailSettings;

        if (mail.ToList == null || mail.ToList.Count < 1)
            return;
        emailPrepare(mail, email: out MimeMessage email, smtp: out SmtpClient smtp);
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
        email.Dispose();
        smtp.Dispose();
    }
    private void emailPrepare(Mail mail, out MimeMessage email, out SmtpClient smtp)
    {
        var attach = char.ConvertFromUtf32(new[] { 0x1F3C0, 0x1F3C3, 0x1F3D0, 0x1F600, 0x1F604, 0x1F609, 0x1F60D, 0x1F60A, 0x1F389, 0x1F44C, 0x1F4AA, 0x1F496, 0x1F4B0, 0x1F64C, 0x1F33C, 0x1F438, 0x1F42F, 0x1F431, 0x1F43E, 0x1F428 }[new Random().Next(18)]);
        email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.SenderFullName + " " + attach, _mailSettings.SenderEmail));
        email.To.AddRange(mail.ToList);
        if (mail.CcList != null && mail.CcList.Any())
            email.Cc.AddRange(mail.CcList);
        if (mail.BccList != null && mail.BccList.Any())
            email.Bcc.AddRange(mail.BccList);

        email.Subject = mail.Subject + " " + attach;
        if (mail.UnsubscribeLink != null)
            email.Headers.Add(field: "List-Unsubscribe", value: $"<{mail.UnsubscribeLink}>");
        BodyBuilder bodyBuilder = new() { TextBody = mail.TextBody, HtmlBody = mail.HtmlBody };

        if (mail.Attachments != null)
            foreach (MimeEntity? attachment in mail.Attachments)
                if (attachment != null)
                    bodyBuilder.Attachments.Add(attachment);

        email.Body = bodyBuilder.ToMessageBody();
        email.Prepare(EncodingConstraint.SevenBit);

        if (!string.IsNullOrEmpty(_mailSettings.DkimPrivateKey) && !string.IsNullOrEmpty(_mailSettings.DkimSelector) && !string.IsNullOrEmpty(_mailSettings.DomainName))
        {
            DkimSigner signer =
                new(key: readPrivateKeyFromPemEncodedString(), _mailSettings.DomainName, _mailSettings.DkimSelector)
                {
                    HeaderCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                    BodyCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                    AgentOrUserIdentifier = $"@{_mailSettings.DomainName}",
                    QueryMethod = "dns/txt"
                };
            HeaderId[] headers = { HeaderId.From, HeaderId.Subject, HeaderId.To };
            signer.Sign(email, headers);
        }

        smtp = new SmtpClient();
        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

        smtp.SslProtocols = SslProtocols.Tls12;
        smtp.Connect(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.Auto);
        if (_mailSettings.AuthenticationRequired)
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
    }

    private AsymmetricKeyParameter readPrivateKeyFromPemEncodedString()
    {
        AsymmetricKeyParameter result;
        string pemEncodedKey =
            "-----BEGIN RSA PRIVATE KEY-----\n" + _mailSettings.DkimPrivateKey + "\n-----END RSA PRIVATE KEY-----";
        using (StringReader stringReader = new(pemEncodedKey))
        {
            PemReader pemReader = new(stringReader);
            object? pemObject = pemReader.ReadObject();
            result = ((AsymmetricCipherKeyPair)pemObject).Private;
        }

        return result;
    }
}
