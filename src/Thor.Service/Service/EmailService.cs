using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;

namespace Thor.Service.Service;

public class EmailService(ILogger<EmailService> logger)
{
    private static readonly ConcurrentDictionary<string, Lazy<SmtpClient>> Factory = new();

    public async Task SendEmailAsync(string email, string subject, string value)
    {
        string smtpHost = SettingService.GetSetting(SettingExtensions.SystemSetting.SmtpAddress);
        string fromAddress = SettingService.GetSetting(SettingExtensions.SystemSetting.EmailAddress);
        string fromPassword = SettingService.GetSetting(SettingExtensions.SystemSetting.EmailPassword);
        
        var mailClient = Factory.GetOrAdd(smtpHost + fromAddress + fromPassword, new Lazy<SmtpClient>(() =>
        {
            var client = new SmtpClient(smtpHost);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(fromAddress, fromPassword); //发送者SMTP密码 并非账号登录密码
            return client;
        })).Value;

        var message = new MailMessage(new MailAddress(fromAddress), new MailAddress(email));

        message.Body = value; //邮件内容
        message.Subject = subject;

        try
        {
            await mailClient.SendMailAsync(message);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Send email failed");
        }

    }
}