using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            services.AddMediatR(Assembly.GetExecutingAssembly());

#if DEBUG
            services.AddDbContextFactory<LotteryContext>(options => options.UseSqlServer(connectionString)
                .EnableSensitiveDataLogging());

#else
            services.AddDbContextFactory<LotteryContext>(options => options.UseSqlServer(connectionString));
#endif
            return services;
        }
    }
}