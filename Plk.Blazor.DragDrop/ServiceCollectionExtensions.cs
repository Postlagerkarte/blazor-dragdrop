using Microsoft.Extensions.DependencyInjection;
using Plk.Blazor.DragDrop.Services;

namespace Plk.Blazor.DragDrop
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
        {
            return services.AddScoped(typeof(DragDropService<>))
                           .AddSingleton<DropzoneGroupService>();
        }
    }
}
