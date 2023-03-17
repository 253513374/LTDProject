using MediatR;
using Wtdl.Model.Entity;

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