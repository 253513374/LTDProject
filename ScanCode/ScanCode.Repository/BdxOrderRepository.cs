using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using ScanCode.Model.Entity.ERP;
using ScanCode.Repository.MediatRHandler.Events;
using ScanCode.Share;
using System.Data;

namespace ScanCode.Repository
{
    public class BdxOrderRepository
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IDbContextFactory<ErpContext> _contextFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbContextFactory">数据库上下文工厂</param>
        /// <param name="Imediator">中介者</param>
        /// <param name="logger">日志记录器</param>
        public BdxOrderRepository(IDbContextFactory<ErpContext> dbContextFactory, IMediator Imediator, ILogger<BdxOrder> logger)
        {
            _contextFactory = dbContextFactory;
            _logger = logger;
            _mediator = Imediator;
        }

        /// <summary>
        /// 根据订单号和查询天数获取单个订单,用作查询订单是否存在
        /// </summary>
        /// <param name="ddno">订单号</param>
        /// <param name="queryDays">查询天数</param>
        /// <returns>订单对象</returns>
        public async Task<BdxOrder> GetSingleAsync(string ddno, int queryDays)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();

                //DateTime startDate = DateTime.Now.AddDays(-queryDays);

                var results = await GetBdxOrderListAsync(ddno.Trim(), queryDays);

                var order = results.Where(w => w.DDNO.Contains(ddno)).FirstOrDefault();

                return order;
            }
            catch (OracleException e)
            {
                _logger.LogError($"查询订单异常{ddno}：{e.Message}");
                // Handle exception...
                return null;
            }
        }

        /// <summary>
        /// 根据订单号获取分组后的订单列表
        /// </summary>
        /// <param name="ddno">订单号</param>
        /// <returns>分组后的订单列表</returns>
        public async Task<List<GroupedBdxOrder>> GetGroupedBdxOrdersDDNOAsync(string ddno, int queryDays)
        {
            var bdxOrders = await GetBdxOrderListAsync(ddno, queryDays);

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
        /// 获取分组后的订单列表
        /// </summary>
        /// <returns>分组后的订单列表</returns>
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
        /// 获取订单列表
        /// </summary>
        /// <returns>订单列表</returns>
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

                return oders;
            }
            catch (OracleException e)
            {
                _logger.LogError($"{e.Message}");
                _ = _mediator?.Publish(new SendEmailEvent()
                {
                    Title = "重要通知：ERP-ORACLE 链接失败",
                    Content = $"ORACLE链接异常：{e.Message}，请及时修复。",
                    Email = ""
                });
                return new List<BdxOrder>();
            }
        }

        /// <summary>
        /// 根据订单号获取订单列表
        /// </summary>
        /// <param name="ddno">订单号</param>
        /// <returns>订单列表</returns>
        public async Task<List<BdxOrder>> GetBdxOrderListAsync(string ddno, int queryDays)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();

                DateTime startDate = DateTime.Now.AddDays(-queryDays);

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
            catch (OracleException e)
            {
                _ = _mediator?.Publish(new SendEmailEvent()
                {
                    Title = "重要通知：ERP-ORACLE 链接失败",
                    Content = $"ORACLE链接异常：{e.Message}，请及时修复。",
                    Email = ""
                });
                _logger.LogError($"模糊查询订单号异常：{e.Message}");
                return new List<BdxOrder>();
            }
        }
    }
}