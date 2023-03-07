using MediatR;
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