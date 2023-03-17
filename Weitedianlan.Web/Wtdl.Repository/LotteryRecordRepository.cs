using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wtdl.Model.Entity;
using Wtdl.Repository.Interface;

namespace Wtdl.Repository
{
    /// <summary>
    /// 这个类中提供了基本的CRUD操作，
    /// 例如添加抽奖记录,更新,查询抽奖记录，查询所有抽奖记录。
    /// </summary>
    public class LotteryRecordRepository : RepositoryBase<LotteryRecord>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<LotteryRecord> _logger;
        private readonly IMediator _mediator;

        public LotteryRecordRepository(IDbContextFactory<LotteryContext> context,
            ILogger<LotteryRecord> logger,
            IMediator mediator
        ) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 返回用户的中奖记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<IEnumerable<LotteryRecord>> GetRecordsAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().Where(x => x.OpenId == openid).ToListAsync();
            }
        }

        /// <summary>
        ///
        /// 返回用户是否有参与过抽奖
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<bool> AnyLotterysAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().AnyAsync(x => x.OpenId == openid);
            }
        }

        /// <summary>
        /// 返回用户的所有抽奖记录列表
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <returns></returns>
        public async Task<bool> GetLotteryRecordsAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().AnyAsync(x => x.OpenId == openid);
            }
        }

        /// <summary>
        /// 返回用户是否有过中奖记录
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<bool> AnySuccessfulRecordsAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().AnyAsync(x => x.OpenId == openid);
            }
        }

        /// <summary>
        /// 返回编号的中奖记录
        /// </summary>
        /// <param name="qrcode"></param>
        /// <returns></returns>
        public async Task<bool> AnyQRCodeRecordsAsync(string qrcode)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().AnyAsync(x => x.QRCode == qrcode);
            }
        }

        /// <summary>
        /// 一个人只能扫一枚标签抽奖。
        /// </summary>
        /// <param name="openid">微信用户openid</param>
        /// <param name="qrcode">标签序号</param>
        /// <returns></returns>
        public async Task<bool> AnyQRCodeRecordsAsync(string openid, string qrcode)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().AnyAsync(x => x.QRCode == qrcode && x.OpenId == openid);
            }
        }

        /// <summary>
        /// 获取最新前90天记录
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<LotteryRecord>> GetLatestLotteryRecordsAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().Where(x => x.CreateTime > DateTime.Now.AddDays(-90)).ToListAsync();
            }
        }

        ///返回总数量
        public async Task<int> GetLotteryRecordsCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().CountAsync();
            }
        }

        /// <summary>
        /// 返回指定用户的抽奖信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable<LotteryRecord>> GetLotteryInfoAsync(string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().Where(x => x.OpenId == openid).ToListAsync();
            }
        }

        /// <summary>
        /// 返回中奖总数
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetLotteryWinRecordsCountAsync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().Where(x => x.IsSuccessPrize == true).CountAsync();
            }
        }
    }
}