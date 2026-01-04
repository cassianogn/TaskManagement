using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Application.TaskItems.Commands.ToggleTaskItem;
using TaskManagement.Application.TaskItems.Queries;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<AddTaskItemCommandHandler>();
            services.AddScoped<ToggleTaskItemCommandHandler>();
            services.AddScoped<GetTaskItemsQueryHandler>();

            return services;
        }
    }
}