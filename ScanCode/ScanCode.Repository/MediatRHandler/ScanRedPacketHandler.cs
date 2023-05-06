using MediatR;
using ScanCode.Model.Entity;

namespace ScanCode.Repository.MediatRHandler
{
    internal class ScanRedPacketHandler : INotificationHandler<RedPacketCinfig>
    {
        public Task Handle(RedPacketCinfig notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}