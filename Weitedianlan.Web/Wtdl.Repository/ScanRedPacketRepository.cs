using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Interface;
using Wtdl.Repository.MediatRHandler.Events;

namespace Wtdl.Repository
{
    public class ScanRedPacketRepository : RepositoryBase<ScanRedPacket>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;

        private readonly ILogger<ScanRedPacket> _logger;
        private readonly IMediator _mediator;

        private JsonSerializerOptions Options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

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
                var count = context.ScanRedPackets.Count();
                if (context.ScanRedPackets.Any())
                {
                    return await context.ScanRedPackets.FirstOrDefaultAsync();//.Result.FirstOrDefaultAsync();

                    //return ob.LastOrDefault();
                }
                else
                {
                    return new ScanRedPacket();
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
                            JsonData = JsonSerializer.Serialize(redPacket, Options),
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
                            JsonData = JsonSerializer.Serialize(redPacket, Options),
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