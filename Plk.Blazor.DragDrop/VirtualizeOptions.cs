using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace Plk.Blazor.DragDrop;
public class VirtualizeOptions<TItem>
{
    /// <summary>
    ///  Gets or sets a value that determines how many additional items will be rendered
    ///  before and after the visible region. This help to reduce the frequency of rendering
    ///  during scrolling. However, higher values mean that more elements will be present
    ///  in the page. Default to 3.
    /// </summary>
    public int OverscanCount { get; set; } = 3;

    /// <summary>
    /// Gets the size of each item in pixels. Defaults to 50px.
    /// </summary>
    public float ItemSize { get; set; } = 50f;

    /// <summary>
    /// Gets or sets the function providing items to the list.
    /// </summary>
    public ItemsProviderDelegate<TItem> ItemsProvider { get; set; }
}
