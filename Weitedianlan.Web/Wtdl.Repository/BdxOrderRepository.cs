using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wtdl.Model.Entity.ERP;
using Wtdl.Repository.Interface;

namespace Wtdl.Repository
{
    public class BdxOrderRepository
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IDbContextFactory<ErpContext> _contextFactory;

        public BdxOrderRepository(IDbContextFactory<ErpContext> dbContextFactory, IMediator Imediator, ILogger<BdxOrder> logger)

        {
            _contextFactory = dbContextFactory;
            _logger = logger;
            _mediator = Imediator;
        }

        ///根据表达式树条件返回单个对象结果
        public async Task<BdxOrder> GetSingleAsync(string ddno)
        {
            using var context = _contextFactory.CreateDbContext();

            DateTime time = DateTime.Now.AddDays(-180);
            var oders = await context.BdxOrders.AsNoTracking().Where(w => w.DDNO.Contains(ddno)).FirstOrDefaultAsync();
            return oders; //await Task.Run(() => context.BdxOrders.AsNoTracking().Where(w=>w.DDRQ>= time.ToString()).FirstOrDefault(where));
        }
    }
}