using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScanCode.Model.Entity;
using ScanCode.Repository.Tools;

namespace ScanCode.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLotteryDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddScoped<PrizeRepository>();
            services.AddScoped<LotteryActivityRepository>();
            services.AddScoped<LotteryRecordRepository>();
            services.AddScoped<VerificationCodeRepository>();
            services.AddScoped<FileUploadRecordRepository>();
            services.AddScoped<AgentRepository>();
            services.AddScoped<WLabelStorageRepository>();
            services.AddScoped<RedPacketRecordRepository>();
            services.AddScoped<ScanRedPacketRepository>();
            services.AddScoped<ActivityPrizeRepository>();
            services.AddScoped<EmailSender>();
            services.AddScoped<OutStorageRepository>();
            services.AddScoped<UserAwardInfoRepository>();

            // var assembly = new[] { Assembly.GetExecutingAssembly(), typeof(LotteryActivity).Assembly };
            // services.AddMediatR(assembly);
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LotteryActivity).Assembly);
            });

#if DEBUG
            services.AddDbContextFactory<LotteryContext>(options => options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

#else
            services.AddDbContextFactory<LotteryContext>(options => options.UseSqlServer(connectionString));
#endif
            return services;
        }

        public static IServiceCollection AddOracleContext(this IServiceCollection services, string contextString)
        {
            services.AddDbContextFactory<ErpContext>(options =>
                options.UseOracle(contextString, b =>
                        b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19))
                    .EnableSensitiveDataLogging());
            services.AddScoped<BdxOrderRepository>();

            return services;
        }
    }
}