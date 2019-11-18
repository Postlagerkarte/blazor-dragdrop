using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.DragDrop.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
        {
            return services.AddScoped<DragDropServiceFactory>();
        }
    }
}
