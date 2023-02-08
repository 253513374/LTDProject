using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;
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
        public async Task<bool> AnyRecordsAsync(string openid)
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
        /// 校验一个人只能中一次奖，不能多次中奖
        /// </summary>
        /// <param name="qrcode"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public async Task<bool> AnyQRCodeRecordsAsync(string qrcode, string openid)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.LotteryRecords.AsNoTracking().AnyAsync(x => x.QRCode == qrcode && x.OpenId == openid);
            }
        }

        // public async Task<bool> Any
    }
}