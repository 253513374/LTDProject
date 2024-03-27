using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScanCode.Model.Entity;

using ScanCode.Repository.Interface;

namespace ScanCode.Repository
{
    public class RedPacketRecordRepository : RepositoryBase<RedPacketRecord>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<RedPacketRecord> _logger;
        private readonly IMediator _mediator;

        public RedPacketRecordRepository(IDbContextFactory<LotteryContext> context,
            ILogger<RedPacketRecord> logger,
            IMediator mediator
        ) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 标签序号是否已经领取过红包，一枚标签只能领取一次红包
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string qrcode)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AnyAsync(x => x.QrCode == qrcode);
            }
        }

        /// <summary>
        /// 验证码是否已经领取过红包，一个验证码只能领取一次红包
        /// </summary>
        /// <param name="qrcode">标签序号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        //public async Task<bool> AnyAsync( )
        //{
        //    using (var context = _contextFactory.CreateDbContext())
        //    {
        //        return await context.RedPacketRecords.AnyAsync();
        //    }
        //}

        ///返回红包发放次数
        public async Task<int> GetRedPacketRecordsCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.CountAsync();
            }
            //   throw new NotImplementedException();
        }

        ///返回红包发放总金额,把分转成元

        public async Task<double> GetRedPacketRecordsTotalAmountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.SumAsync(x => Convert.ToDouble(x.TotalAmount)) / 100;
            }
            //   throw new NotImplementedException();
        }

        /// <summary>
        /// 返回指定用户领取红包记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<RedPacketRecord>> GetUserRedPacketInfoAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AsNoTracking().Where(x => x.ReOpenId == openid).ToListAsync();
                //return await context.RedPacketRecords.AsNoTracking().Where(x => x.OpenId == openid);
            }
            //   throw new NotImplementedException();
            // throw new NotImplementedException();
        }

        /// <summary>
        /// 返回二维码领取红包的数量
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<int> FindAsync(string qrcode)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AsNoTracking()
                    .CountAsync(c => c.QrCode == qrcode);
                //.Where(x => x.QrCode == qrcode).Select(x => x.QrCode).ToListAsync();
            }
        }

        public async Task<int> FindUserLimt(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AsNoTracking()
                    .CountAsync(c => c.ReOpenId == openid);
                //.Where(x => x.ReOpenId == openid).Select(x => x.ReOpenId).ToListAsync();
            }
        }

        //这个函数根据字段ReOpenId分组统计，每个ReOpenId领取红包金额TotalAmount（这个需要转换int才能使用sum,保留两位小数）
        public async Task<List<RedPacketRecord>> GetRedPacketRecordsGroupByReOpenIdAsync(int topCount)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AsNoTracking()

                    .GroupBy(g => g.ReOpenId)
                    .Select(s => new RedPacketRecord()
                    {
                        ReOpenId = s.Key,
                        TotalAmount = Math.Round(s.Sum(s => int.Parse(s.TotalAmount)) / 100.0, 2).ToString()
                    })
                    .OrderByDescending(t => t.TotalAmount)
                    .Take(topCount)
                    .ToListAsync();
            }
        }

        //返回指定用户领取红包的明细
        public async Task<List<RedPacketRecord>> GetUserRedPacketDetailAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AsNoTracking()
                    .Where(x => x.ReOpenId == openid)

                    .ToListAsync();
            }
        }
    }
}