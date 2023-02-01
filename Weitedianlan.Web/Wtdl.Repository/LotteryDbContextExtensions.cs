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
            services.AddScoped<PrizeRepository>();
            services.AddScoped<LotteryActivityRepository>();
            services.AddScoped<LotteryRecordRepository>();
            services.AddScoped<VerificationCodeRepository>();
            services.AddScoped<FileUploadRecordRepository>();
            services.AddScoped<AgentRepository>();
            services.AddScoped<WLabelStorageRepository>();

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