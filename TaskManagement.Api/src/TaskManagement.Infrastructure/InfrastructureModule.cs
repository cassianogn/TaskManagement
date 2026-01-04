using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.TaskItems;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Repository;

namespace TaskManagement.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<TaskManagementDbContext>(options =>
                options.UseInMemoryDatabase("TaskManagementDb"));

            services.AddScoped<ITaskItemRepository, TaskItemRepository>();
            return services;
        }
    }
}
