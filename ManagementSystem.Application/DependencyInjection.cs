using Microsoft.Extensions.DependencyInjection;
using ManagementSystem.Application.Common.Interfaces;
using ManagementSystem.Application.Students;

namespace ManagementSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>(); 
            services.AddScoped<IPdfService, PdfService>();

            return services;
        }
    }
}