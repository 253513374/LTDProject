using MediatR;
using Wtdl.Model.Entity;

namespace Wtdl.Repository.MediatRHandler
{
    internal class ScanRedPacketHandler : INotificationHandler<RedPacketCinfig>
    {
        public Task Handle(RedPacketCinfig notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}