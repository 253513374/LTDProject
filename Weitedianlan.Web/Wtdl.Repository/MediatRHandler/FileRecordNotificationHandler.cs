using MediatR;
using Wtdl.Model.Entity;

namespace Wtdl.Repository.MediatRHandler
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
            }

            return Task.CompletedTask;
        }
    }
}