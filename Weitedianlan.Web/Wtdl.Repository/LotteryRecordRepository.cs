using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Weitedianlan.Model.Entity;

namespace Wtdl.Repository
{
    /// <summary>
    /// 这个类中提供了基本的CRUD操作，
    /// 例如添加抽奖记录,更新,查询抽奖记录，查询所有抽奖记录。
    /// </summary>
    public class LotteryRecordRepository
    {
        private readonly LotteryContext _context;

        public LotteryRecordRepository(LotteryContext context)
        {
            _context = context;
        }

        public void Add(LotteryRecord record)
        {
            _context.LotteryRecords.Add(record);
            _context.SaveChanges();
        }

        public void Update(LotteryRecord record)
        {
            _context.LotteryRecords.Update(record);
            _context.SaveChanges();
        }

        public LotteryRecord Get(int id)
        {
            return _context.LotteryRecords.FirstOrDefault(r => r.Id == id);
        }

        public List<LotteryRecord> GetAll()
        {
            return _context.LotteryRecords.ToList();
        }
    }
}