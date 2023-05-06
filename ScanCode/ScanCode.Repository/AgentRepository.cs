using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using ScanCode.Model.Entity;
using ScanCode.Repository.Interface;

namespace ScanCode.Repository
{
    public class AgentRepository : RepositoryBase<Agent>
    {
        private readonly IDbContextFactory<LotteryContext> _contextFactory;
        private readonly ILogger<Agent> _logger;
        private readonly IMediator _mediator;

        public AgentRepository(IDbContextFactory<LotteryContext> context,
            IMediator mediator,
            ILogger<Agent> logger) : base(context, mediator, logger)
        {
            _contextFactory = context;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<List<string>> GetAgentsByANameGroup(string AName)
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                return await dbContext.Agents
                    .Where(a => a.AName == AName)
                    .GroupBy(a => a.AName)
                    .Select(g => g.Key)
                    .ToListAsync();
            }
        }

        public async Task<Agent> FindSingleAgentAsync(string aid)
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                return await dbContext.Agents.AsNoTracking().Where(f => f.AID == aid).FirstOrDefaultAsync();
            }
        }

        public async Task<Agent> AddAgentAsync(Agent agent)
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                await dbContext.Agents.AddAsync(agent);
                await dbContext.SaveChangesAsync();
                return agent;
            }
        }

        public async Task<Agent> UpdateAgentAsync(Agent agent)
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                dbContext.Agents.Update(agent);
                await dbContext.SaveChangesAsync();
                return agent;
            }
        }

        //public async Task<List<Participant>> GetParticipantsAsync()
        //{
        //    using (var dbContext = _contextFactory.CreateDbContext())
        //    {
        //        return await dbContext.Agents.AsNoTracking().GroupBy(o => o.AName).Select(s => new Participant()
        //        {
        //            PName = s.Key,
        //            Identifier = "客户信息",
        //        }).Take(10).Distinct().ToListAsync();
        //    }
        //}

        public async Task<Agent> DeleteAgentAsync(int id)
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                var agent = await dbContext.Agents.FindAsync(id);
                dbContext.Agents.Remove(agent);
                await dbContext.SaveChangesAsync();
                return agent;
            }
        }

        /// <summary>
        /// 获取客户名称
        /// </summary>
        /// <param name="orderNumbels"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string?> GetAgentName(string orderNumbels)
        {
            using (var dbContext = _contextFactory.CreateDbContext())
            {
                var result = await dbContext.Agents.AsNoTracking().Where(f => f.AID.Contains(orderNumbels.Trim())).Select(s => s.AName)
                    .FirstOrDefaultAsync();

                return result;
            }
            //throw new NotImplementedException();
        }
    }
}