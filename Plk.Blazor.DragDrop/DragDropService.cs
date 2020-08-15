using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
            ActiveItem = default(T);
            ActiveSpacerId = null;
            Items = null;
        }

        public bool ShouldRender { get; set; } = true;
    }
}

