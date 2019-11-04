using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.DragDrop.Core
{
    public class DraggableItem
    {
        private readonly DragDropService dragDropService;

        public DraggableItem(DragDropService dragDropService)
        {
            this.dragDropService = dragDropService;
        }

        public RenderFragment<DraggableItem> RenderFragement { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int DropzoneId { get; set; }

        public int OriginDropzoneId { get; set; }

        public dynamic Tag { get; set; }

        public Func<DraggableItem, bool> AllowDrag { get; set; }

        public int OrderPosition
        {
            get
            {
                return dragDropService.GetOrderPosition(DropzoneId, Id);
            }
        }

    }
}
