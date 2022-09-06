using Microsoft.Extensions.DependencyInjection;

namespace Plk.Blazor.DragDrop;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorDragDrop(this IServiceCollection services)
    {
        return services.AddScoped(typeof(DragDropService<>));
    }
}
