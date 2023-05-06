using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using ScanCode.Model.Entity.ERP;
using ScanCode.Share;
using System.Data;

namespace ScanCode.Repository
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

        public async Task<List<GroupedBdxOrder>> GetGroupedBdxOrdersDDNOAsync(string ddno)
        {
            var bdxOrders = await GetBdxOrderListAsync(ddno);

            var groupedOrders = bdxOrders
                .GroupBy(order => order.DDNO)
                .Select(group => new GroupedBdxOrder
                {
                    DDNO = group.Key,
                    KH = group.FirstOrDefault()?.KH,
                    DDRQ = group.FirstOrDefault()?.DDRQ,
                    THRQ = group.FirstOrDefault()?.THRQ,
                    DW = group.FirstOrDefault()?.DW,
                    TotalSL = group.Sum(order =>
                    {
                        int.TryParse(order.SL, out int parsedSL);
                        return parsedSL;
                    }),
                })
                .ToList();

            return groupedOrders;
        }

        public async Task<List<GroupedBdxOrder>> GetGroupedBdxOrdersAsync()
        {
            // 获取 ERP 出库单列表
            List<BdxOrder> bdxOrders = await GetBdxOrdersAsync();

            // 使用 LINQ 查询根据订单分组并统计每个组的 SL 总和，以及获取 KH、DDRQ 和 THRQ 属性
            var groupedOrders = bdxOrders
                .GroupBy(order => order.DDNO)
                .Select(group => new GroupedBdxOrder
                {
                    DDNO = group.Key,
                    KH = group.FirstOrDefault()?.KH,
                    DDRQ = group.FirstOrDefault()?.DDRQ,
                    THRQ = group.FirstOrDefault()?.THRQ,
                    DW = group.FirstOrDefault()?.DW,
                    TotalSL = group.Sum(order =>
                    {
                        int.TryParse(order.SL, out int parsedSL);
                        return parsedSL;
                    }),
                })
                .ToList();

            return groupedOrders;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private async Task<List<BdxOrder>> GetBdxOrdersAsync()
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();

                DateTime startDate = DateTime.Now.AddDays(-60);
                DateTime endDate = DateTime.Now.AddDays(2);

                // 您可能需要根据您的表结构和字段类型调整此 SQL 查询
                var sql = @"
                            SELECT DDNO, DDRQ, KH, XH, GGXH, SL, DW, DJ, THRQ, YS
                            FROM T_BDX_ORDER
                            WHERE TRUNC(TO_DATE(DDRQ, 'YYYY-MM-DD HH24:MI:SS')) >= :startDate
                            AND TRUNC(TO_DATE(DDRQ, 'YYYY-MM-DD HH24:MI:SS')) <= :endDate
                            AND (THRQ IS NULL OR LENGTH(THRQ) = 0)";

                var parameters = new[]
                {
                    new OracleParameter("startDate", OracleDbType.Date) { Value = startDate },
                    new OracleParameter("endDate", OracleDbType.Date) { Value = endDate },
                };

                var oders = await context.BdxOrders.FromSqlRaw(sql, parameters).ToListAsync();

                //var oders = await context.BdxOrders.AsNoTracking()
                //     //.Where(order => DateTime.Parse(order.DDRQ) >= startDate && DateTime.Parse(order.DDRQ) <= endDate)
                //     //.Where(order => EF.Functions.TruncateTime(order.DDRQ) >= startDate.Date && EF.Functions.TruncateTime(order.DDRQ) <= endDate.Date)
                //    .Where(order => string.IsNullOrWhiteSpace(order.THRQ))
                //    .ToListAsync();

                return oders;
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message}");
                return new List<BdxOrder>();
            }

            //throw new NotImplementedException();
        }

        ///// <summary>
        ///// 返回订单号列表
        ///// </summary>
        ///// <param name="ddno"></param>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        ////public async Task<List<BdxOrder>> GetBdxOrderListAsync(string ddno)
        ////{
        ////    using var context = _contextFactory.CreateDbContext();

        ////    DateTime time = DateTime.Now.AddDays(-180);
        ////    var oders = await context.BdxOrders.AsNoTracking().Where(w => w.DDNO.Contains(ddno)).ToListAsync();
        ////    return oders;
        ////}

        /// <summary>
        /// 返回订单号列表
        /// </summary>
        /// <param name="ddno"></param>
        /// <returns></returns>
        public async Task<List<BdxOrder>> GetBdxOrderListAsync(string ddno)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();

                DateTime startDate = DateTime.Now.AddDays(-60);

                // 您可能需要根据您的表结构和字段类型调整此 SQL 查询
                var sql = @"
                            SELECT DDNO, DDRQ, KH, XH, GGXH, SL, DW, DJ, THRQ, YS
                            FROM T_BDX_ORDER
                            WHERE TRUNC(TO_DATE(DDRQ, 'YYYY-MM-DD HH24:MI:SS')) >= :startDate
                            AND DDNO LIKE :ddNo";

                var parameters = new[]
                {
                    new OracleParameter("startDate", OracleDbType.Date) { Value = startDate },
                    new OracleParameter("ddNo", OracleDbType.NVarchar2) { Value = $"%{ddno}%" },
                };

                var oders = await context.BdxOrders.FromSqlRaw(sql, parameters).ToListAsync();

                return oders;
            }
            catch (Exception e)
            {
                //Console.WriteLine(e);

                _logger.LogError($"模糊查询订单号异常：{e.Message}");
                // throw;
                return new List<BdxOrder>();
            }
        }
    }
}