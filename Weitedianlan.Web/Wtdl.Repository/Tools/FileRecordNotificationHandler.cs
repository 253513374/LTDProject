using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.Tools
{
    internal class FileRecordNotificationHandler : INotificationHandler<FileUploadRecord>
    {
        public Task Handle(FileUploadRecord record, CancellationToken cancellationToken)
        {
            //在这里处理通知。例如，您可以发送电子邮件或记录事件

            Console.WriteLine($"接收到删除文件信息{record.FilePath}");
            if (File.Exists(record.FilePath))
            {
                File.Delete(record.FilePath);
                Console.WriteLine($"成功删除文件:{record.FilePath}");
                //_logger.LogInformation($"文件已经存在，删除文件:{file.Name}");
            }

            return Task.CompletedTask;
        }
    }
}