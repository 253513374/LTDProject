using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Interface;
using Wtdl.Repository.MediatRHandler.Events;
using Wtdl.Repository.Utility;

namespace Wtdl.Repository
{
    /// <summary>
    /// 现金红包配置
    /// </summary>
    public class ScanRedPacketRepository : RepositoryBase<ScanRedPacket>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;

        private readonly ILogger<ScanRedPacket> _logger;
        private readonly IMediator _mediator;

        public ScanRedPacketRepository(IDbContextFactory<LotteryContext> context, IMediator mediator,
                ILogger<ScanRedPacket> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ScanRedPacket> FindScanRedPacketAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                if (context.ScanRedPackets.Any())
                {
                    return await context.ScanRedPackets.AsNoTracking().FirstOrDefaultAsync();//.Result.FirstOrDefaultAsync();
                }
                else
                {
                    return new ScanRedPacket();
                }
            }
        }

        /// <summary>
        /// 判断红包活动是否激活
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AnyRedPacketActiveAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                if (context.ScanRedPackets.Any())
                {
                    return await context.ScanRedPackets.Select(s => s.IsActivity).FirstOrDefaultAsync();//.Result.FirstOrDefaultAsync();
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<int> UpdateOrInsert(ScanRedPacket redPacket)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var findresult = await context.ScanRedPackets.AsNoTracking().AnyAsync(x => x.ScanRedPacketGuid == redPacket.ScanRedPacketGuid);

                if (findresult)
                {
                    context.ScanRedPackets.Update(redPacket);

                    var s = await context.SaveChangesAsync();
                    if (s > 0)
                    {
                        await _mediator.Publish(new LoggerEvent()
                        {
                            TypeData = redPacket.GetType(),
                            JsonData = GlobalUtility.SerializeObject(redPacket),
                            OperationType = OperationType.Update
                        });
                        return s;
                    }
                }
                else
                {
                    await context.ScanRedPackets.AddAsync(redPacket);
                    //  await _mediator.Publish(redPacket);
                    //  return await context.SaveChangesAsync();

                    var s = await context.SaveChangesAsync();
                    if (s > 0)
                    {
                        await _mediator.Publish(new LoggerEvent()
                        {
                            TypeData = redPacket.GetType(),
                            JsonData = GlobalUtility.SerializeObject(redPacket),
                            OperationType = OperationType.Insert
                        });
                        return s;
                    }
                }

                return 0;
            }
        }
    }
}