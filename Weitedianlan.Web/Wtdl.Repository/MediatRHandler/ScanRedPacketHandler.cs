using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository.MediatRHandler
{
    internal class ScanRedPacketHandler : INotificationHandler<ScanRedPacket>
    {
        public Task Handle(ScanRedPacket notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}