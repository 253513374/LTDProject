using EFCore.BulkExtensions;
using MediatR;

namespace ScanCode.Repository.MediatRHandler.Events
{
    public class LoggerEvent : INotification
    {
        public LoggerEvent()
        {
            CreateTime = DateTime.Now;
        }

        public Type TypeData { get; set; }

        public string JsonData { get; set; }

        public OperationType OperationType { get; set; }

        public DateTime CreateTime { get; set; }
    }
}