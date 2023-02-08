using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.MediatRHandler.Events;

namespace Wtdl.Repository.MediatRHandler
{
    public class LoggerEventHnadler : INotificationHandler<LoggerEvent>
    {
        private readonly ILogger<LoggerEvent> _Logger;

        public LoggerEventHnadler(ILogger<LoggerEvent> logger)
        {
            _Logger = logger;
        }

        public async Task Handle(LoggerEvent notification, CancellationToken cancellationToken)
        {
            //在这里处理通知。例如，您可以发送电子邮件或记录事件

            var loggermsg =
                $"{notification.CreateTime}接收到{notification.TypeData.ToString()}通知,{notification.OperationType.ToString()} 处理, {notification.JsonData}";
            await LogAsync(loggermsg);
            return;
        }

        public async Task LogAsync(string message)
        {
            _Logger.LogInformation(message);
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var fileName = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
            var path = Path.Combine(directory, fileName);
            using (var writer = File.AppendText(path))
            {
                await writer.WriteLineAsync($"{DateTime.Now}: {message}");
            }
        }
    }
}