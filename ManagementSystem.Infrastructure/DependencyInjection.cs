using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Infrastructure.Persistence;
using ManagementSystem.Infrastructure.Persistence.Repositories;

namespace ManagementSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString)
        {
            services.AddDbContext<ManagementDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IStudentRepository, StudentRepository>();

            return services;
        }
    }
}
