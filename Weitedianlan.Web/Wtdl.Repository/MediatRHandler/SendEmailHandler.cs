using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.MediatRHandler.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Wtdl.Repository.Tools;

namespace Wtdl.Repository.MediatRHandler
{
    public class SendEmailHandler : INotificationHandler<SendEmailEvent>
    {
        //private readonly IConfiguration _configuration;
        private readonly EmailSender _emailSender;

        public SendEmailHandler(EmailSender emailSender, IConfiguration configuration)
        {
            _emailSender = emailSender;
            //_configuration = configuration;
            // configuration.GetSection("EmailSettings").Bind(_emailSettings);
        }

        public async Task Handle(SendEmailEvent notification, CancellationToken cancellationToken)
        {
            await _emailSender.SendEmailAsync(notification.Email, notification.Title, notification.Content);
            // return Task.CompletedTask;
            //  throw new NotImplementedException();
        }

        ///// <summary>
        ///// 发送邮件
        ///// </summary>
        ///// <param name="email">收件人地址 </param>
        ///// <param name="subject">主题</param>
        ///// <param name="message">邮件内容</param>
        ///// <returns></returns>
        //private async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    //var smtpServer = _configuration.GetSection("EmailSettings")["SmtpServer"];
        //    //var port = Convert.ToInt32(_configuration.GetSection("EmailSettings")["Port"]);
        //    //var userName = _configuration.GetSection("EmailSettings")["UserName"];
        //    //var password = _configuration.GetSection("EmailSettings")["Password"];
        //    //"smtp.example.com"

        //    try
        //    {
        //        var client = new SmtpClient(_emailSettings.SmtpServer)
        //        {
        //            Port = _emailSettings.Port,
        //            Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
        //            EnableSsl = true,
        //            DeliveryMethod = SmtpDeliveryMethod.Network,
        //            UseDefaultCredentials = false
        //        };

        //        var mailMessage = new MailMessage
        //        {
        //            From = new MailAddress(_emailSettings.From),
        //            To = { email },
        //            Subject = subject,
        //            Body = message,
        //            IsBodyHtml = true
        //        };

        //        await client.SendMailAsync(mailMessage);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}
    }
}