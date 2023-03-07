using MediatR;

namespace Wtdl.Repository.MediatRHandler.Events
{
    /// <summary>
    /// 发送邮件事件
    /// </summary>
    public class SendEmailEvent : INotification
    {
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class EmailSettings
    {
        public string From { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}