using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wtdl.Model.Entity;
using Wtdl.Repository.Interface;

namespace Wtdl.Repository
{
    public class UserAwardInfoRepository : RepositoryBase<UserAwardInfo>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<UserAwardInfo> _logger;
        private readonly IMediator _mediator;

        public UserAwardInfoRepository(IDbContextFactory<LotteryContext> dbContextFactory, IMediator Imediator, ILogger<UserAwardInfo> logger) : base(dbContextFactory, Imediator, logger)
        {
            _contextFactory = dbContextFactory;
            _mediator = Imediator;
            _logger = logger;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<List<UserAwardInfo?>> GetByOpenIdAsync(string openid)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.UserAwardInfos.AsNoTracking().Where(x => x.WeChatOpenId == openid).ToListAsync();
            // throw new NotImplementedException();
        }
    }
}