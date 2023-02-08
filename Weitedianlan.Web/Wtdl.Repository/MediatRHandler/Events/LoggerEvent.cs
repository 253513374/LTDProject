using EFCore.BulkExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wtdl.Repository.MediatRHandler.Events
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