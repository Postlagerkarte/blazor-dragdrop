using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.DragDrop.Core
{
    public class DraggableItem
    {
        public RenderFragment RenderFragement { get; set; }

        public int Id { get; set; }

        public int DropzoneId { get; set; }

        public dynamic Tag { get; set; }
    }
}
