using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Wtdl.Model.Entity;
using Wtdl.Repository.Tools;

namespace Wtdl.Repository
{
    public static class LotteryDbContextExtensions
    {
        public static IServiceCollection AddLotteryDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddTransient<PrizeRepository>();
            services.AddTransient<LotteryActivityRepository>();
            services.AddTransient<LotteryRecordRepository>();
            services.AddTransient<VerificationCodeRepository>();
            services.AddTransient<FileUploadRecordRepository>();
            services.AddTransient<AgentRepository>();
            services.AddTransient<WLabelStorageRepository>();
            services.AddTransient<RedPacketRecordRepository>();
            services.AddScoped<ScanRedPacketRepository>();
            services.AddScoped<ActivityPrizeRepository>();
            services.AddScoped<EmailSender>();
            services.AddScoped<OutStorageRepository>();
            services.AddScoped<UserAwardInfoRepository>();

            var assembly = new[] { Assembly.GetExecutingAssembly(), typeof(LotteryActivity).Assembly };
            services.AddMediatR(assembly);

#if DEBUG
            services.AddDbContextFactory<LotteryContext>(options => options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

#else
            services.AddDbContextFactory<LotteryContext>(options => options.UseSqlServer(connectionString));
#endif
            return services;
        }

        public static IServiceCollection AddOracleContext(this IServiceCollection services,
            string contextString)
        {
            services.AddDbContextFactory<ErpContext>(options =>
                options.UseOracle(contextString, b =>
                        b.UseOracleSQLCompatibility("11"))
                    .EnableSensitiveDataLogging());
            services.AddScoped<BdxOrderRepository>();
            return services;
        }
    }
}