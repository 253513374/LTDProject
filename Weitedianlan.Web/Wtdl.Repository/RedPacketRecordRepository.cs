﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Interface;

namespace Wtdl.Repository
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
        public async Task<bool> AnyAsync(string qrcode, string code)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.RedPacketRecords.AnyAsync(x => x.QrCode == qrcode && x.VerificationCode == code);
            }
        }
    }
}