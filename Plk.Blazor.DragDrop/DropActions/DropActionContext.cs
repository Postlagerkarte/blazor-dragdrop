using System.Collections.Generic;

namespace Plk.Blazor.DragDrop.DropActions
{
    public class DropActionContext<TItem>
    {
        public Dropzone<TItem> Sender { get; init; }
        public TItem ActiveItem { get; init; }
        public IList<TItem> SourceItems { get; init; }
        public IList<TItem> TargetItems { get; init; }
        public int? TargetItemIndex { get; init; }
    }
}
