using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.DragDrop.Core
{
    public class DataItem
    {
        public RenderFragment RenderFragement { get; set; }

        public int DraggableId { get; set; }
    }
}
