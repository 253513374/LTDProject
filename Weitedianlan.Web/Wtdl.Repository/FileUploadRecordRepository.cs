using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Weitedianlan.Model.Entity;
using Wtdl.Repository.Interface;

namespace Wtdl.Repository
{
    public class FileUploadRecordRepository : RepositoryBase<FileUploadRecord>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<FileUploadRecord> _logger;
        private readonly IMediator _mediator;
        //private IWebHostEnvironment _env;

        public FileUploadRecordRepository(IDbContextFactory<LotteryContext> context,
            IMediator mediator,
            ILogger<FileUploadRecord> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// 根据文件哈希值返回文件信息
        /// </summary>
        /// <param name="filehash"></param>
        /// <returns></returns>
        //public bool GetByHash(string filehash)
        //{
        //    using var context = _contextFactory.CreateDbContext();
        //    return context.FileUploadRecords.Any(a => a.FileHash == filehash);
        //    //  return await context.FileUploadRecords.Where(w=>w.FileHash== filehash);
        //}

        //public async Task<FileUploadRecord> GetByFileName(string filename)
        //{
        //    using var context = _contextFactory.CreateDbContext();

        //    var result = context.FileUploadRecords.FirstOrDefault(x => x.FileName == filename);
        //    return result;
        //}
    }
}