using System.Collections.Generic;
using Plk.Blazor.DragDrop.DropActions;

namespace Plk.Blazor.DragDrop
{
    public class DropzoneSelectDropActionArgs<TItem>
    {
        public Dropzone<TItem> Sender { get; set; }
        public TItem ActiveItem { get; set; }
        public IList<TItem> SourceItems { get; set; }
        public IList<TItem> TargetItems { get; set; }
        public int? TargetItemIndex { get; set; }

        public IDropAction<TItem> DropAction { get; set; }
    }
}
