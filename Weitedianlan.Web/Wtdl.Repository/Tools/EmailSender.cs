using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Utils;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Wtdl.Repository.MediatRHandler.Events;
using Microsoft.Extensions.Options;

namespace Wtdl.Repository.Tools
{
    public class EmailSender
    {
        // public IWebHostEnvironment Environment { get; }
        private readonly ILogger<EmailSender> Logger;

        private readonly EmailSettings options;

        public EmailSender(ILogger<EmailSender> logger, IOptions<EmailSettings> option)
        {
            //Environment = environment;
            options = option.Value;
            Logger = logger;
        }

        public async Task SendEmailAsync(string toemail, string subject, string message)
        {
            if (toemail == "")
            {
                toemail = "253513374@qq.com";
            }
            // 设置邮件内容
            var mailmessage = new MimeMessage();
            mailmessage.From.Add(new MailboxAddress("溯源系统紧急通知", options.From));//添加发件人
            mailmessage.To.Add(new MailboxAddress("optionsUserName", address: toemail));//收件人
            mailmessage.Subject = subject;

            var builder = new BodyBuilder();
            builder.TextBody = message;
            // string.Format(@"<p>你好,{0} </p>
            //<p>请在24小时内点击右侧连接重置你得密码，逾期链接将失效。 <a href=""{1}"" target=""_blank"">重置密码 </a></p>
            //<p>如无法打开连接,请复制以下连接到浏览器地址栏中打开</p>
            // <p>""{1}""</p>
            //<center><img src=""cid:{2}""></center>", user.UserName, url, image.ContentId);

            mailmessage.Body = builder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect(options.SmtpServer, options.Port, SecureSocketOptions.SslOnConnect);
                smtpClient.Authenticate(options.UserName, options.Password);

                await smtpClient.SendAsync(mailmessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}