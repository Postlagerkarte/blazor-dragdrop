using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Plk.Blazor.DragDrop.Test")]
namespace Plk.Blazor.DragDrop
{
    internal class DragDropService<T>
    {
        /// <summary>
        /// Currently Active Item
        /// </summary>
        public T ActiveItem { get; set; }

        /// <summary>
        /// The item the active item is hovering above.
        /// </summary>
        public T DragTargetItem { get; set; }

        /// <summary>
        /// Holds a reference to the items of the dropzone in which the drag operation originated
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Holds the id of the Active Spacing div
        /// </summary>
        public int? ActiveSpacerId { get; set; }

        /// <summary>
        /// Resets the service to initial state
        /// </summary>
        public void Reset()
        {
            ShouldRender = true;
            ActiveItem = default;
            ActiveSpacerId = null;
            Items = null;
            DragTargetItem = default;

            StateHasChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool ShouldRender { get; set; } = true;

        // Notify subscribers that there is a need for rerender
        public EventHandler StateHasChanged { get; set; }
    }
}
