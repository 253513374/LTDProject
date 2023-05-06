using CsvHelper;
using CsvHelper.Configuration;
using EFCore.BulkExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScanCode.Model.Entity;
using ScanCode.Repository.Data;
using ScanCode.Repository.Interface;
using ScanCode.Repository.Tools;
using ScanCode.Repository.Utility;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace ScanCode.Repository
{
    public class VerificationCodeRepository : RepositoryBase<VerificationCode>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;

        private readonly ILogger<VerificationCode> _logger;
        private readonly IMediator _mediator;

        public VerificationCodeRepository(IDbContextFactory<LotteryContext> context, IMediator mediator, ILogger<VerificationCode> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        private async Task<InsertResult> BulkInsertTask(List<VerificationCode> DataList)
        {
            var taskList = new List<Task>();
            var chunkSize = 100000;
            var chunkCount = DataList.Count / chunkSize + (DataList.Count % chunkSize == 0 ? 0 : 1);

            for (var i = 0; i < chunkCount; i++)
            {
                var chunkStart = i * chunkSize;
                var chunkEnd = chunkStart + chunkSize;
                if (chunkEnd > DataList.Count) chunkEnd = DataList.Count;

                var chunk = DataList.GetRange(chunkStart, chunkEnd - chunkStart);
                taskList.Add(Task.Factory.StartNew(async (obj) =>
                {
                    var index = (int)obj;
                    using (var context = _contextFactory.CreateDbContext())
                    {
                        var stopwatch = new Stopwatch();
                        stopwatch.Start();
                        await context.BulkInsertAsync(chunk);
                        stopwatch.Stop();

                        _logger.LogInformation($"TASK-[{index}]:数据成功写入数据库{chunk.Count}:{chunk.Last().QRCode} 耗时：{stopwatch.ElapsedMilliseconds}毫秒");
                    }
                }, i, TaskCreationOptions.LongRunning));
            }

            await Task.WhenAll(taskList.ToArray());
            return new InsertResult { IsSuccess = true, Message = "数据成功写入数据库", SuccessCount = DataList.Count };
        }

        private async Task<InsertResult> BulkInsert(List<VerificationCode> lists)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                await context.BulkInsertAsync(lists);
                //使用事务功能，保证数据一致性
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //lists = lists.

                        //批量插入，遇见重复数据就更新数据
                        await context.BulkInsertAsync(lists);
                        transaction.Commit();
                        _logger.LogInformation($"数据成功写入数据库");
                        //        // _mediator.Send()
                        return new InsertResult { IsSuccess = true, Message = "数据成功写入数据库", SuccessCount = lists.Count };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();

                        _logger.LogError(ex.ToString());
                        return new InsertResult { IsSuccess = false, Message = ex.Message.ToString() };
                        // throw ex;
                    }
                }
            }
        }

        public async Task<InsertResult> BulkInsertAsync(string filePath)
        {
            try
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = false,
                    MissingFieldFound = null
                };
                using (var reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    using (var csv = new CsvReader(reader, config))
                    {
                        var filehash = GlobalUtility.ComputeFileHash(filePath);

                        csv.Context.RegisterClassMap(new VCodeCsvHelperMap(filehash));

                        var vcodeList = csv.GetRecords<VerificationCode>().ToList();

                        return await BulkInsertTask(vcodeList);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                return new InsertResult { IsSuccess = false, Message = e.Message.ToString() };
                // throw;
            }
        }

        /// <summary>
        /// 验证码与标签序号是否对应
        /// </summary>
        /// <param name="qrcode"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(string qrcode, string captcha)
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                return await context.VerificationCodes.AnyAsync(x => x.QRCode == qrcode && x.Captcha == captcha);
            }
        }
    }
}