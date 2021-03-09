using System.Collections.Generic;

namespace Plk.Blazor.DragDrop.DropActions
{
    public class DropActionContext<TItem>
    {
        public Dropzone<TItem> Sender { get; set; }
        public TItem ActiveItem { get; set; }
        public IList<TItem> SourceItems { get; set; }
        public IList<TItem> TargetItems { get; set; }
        public int? TargetItemIndex { get; set; }
    }
}
