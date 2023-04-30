using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wtdl.Model.Entity;
using Wtdl.Repository.Interface;
using Wtdl.Repository.MediatRHandler.Events;
using Wtdl.Repository.Utility;

namespace Wtdl.Repository
{
    /// <summary>
    /// 现金红包配置
    /// </summary>
    public class ScanRedPacketRepository : RepositoryBase<RedPacketCinfig>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;

        private readonly ILogger<RedPacketCinfig> _logger;
        private readonly IMediator _mediator;

        public ScanRedPacketRepository(IDbContextFactory<LotteryContext> context, IMediator mediator,
                ILogger<RedPacketCinfig> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<RedPacketCinfig> FindScanRedPacketAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                if (context.ScanRedPackets.Any())
                {
                    return await context.ScanRedPackets.AsNoTracking().FirstOrDefaultAsync();//.Result.FirstOrDefaultAsync();
                }
                else
                {
                    return new RedPacketCinfig();
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

        public async Task<(int, string)> UpdateOrInsert(List<RedPacketCinfig> redPackets)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                //var findresult = await context.ScanRedPackets.AsNoTracking().AnyAsync(x => x.ScanRedPacketGuid == redPacket.ScanRedPacketGuid);

                try
                {
                    await context.BulkInsertOrUpdateAsync(redPackets);

                    return (2, "");
                }
                catch (Exception e)
                {
                    _logger.LogError($"红包配置异常：{e.Message}");
                    return (0, $"{e.Message}");
                }
            }
        }

        /// <summary>
        /// 获取第一个红包的配置
        /// </summary>
        /// <returns></returns>
        public async Task<RedPacketCinfig?> FindRedPacketQRCodeOptionsAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var QRCodeOptions = await context.ScanRedPackets.Where(w => w.RedPacketConfigType == RedPacketConfigType.QRCode).AsNoTracking().FirstOrDefaultAsync();

                if (QRCodeOptions is null)
                {
                    return new RedPacketCinfig() { RedPacketConfigType = RedPacketConfigType.QRCode };
                }

                return QRCodeOptions;
            }
        }

        /// <summary>
        /// 获取第二个红包的配置
        /// </summary>
        /// <returns></returns>
        public async Task<RedPacketCinfig?> FindRedPacketCaptchaOptionsAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                var CaptchaOptions = await context.ScanRedPackets.Where(w => w.RedPacketConfigType == RedPacketConfigType.Captcha).AsNoTracking().FirstOrDefaultAsync();

                if (CaptchaOptions is null)
                {
                    return new RedPacketCinfig() { RedPacketConfigType = RedPacketConfigType.Captcha };
                }

                return CaptchaOptions;
            }
        }
    }
}