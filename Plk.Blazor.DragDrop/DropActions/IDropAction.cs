using System.Collections;
using System.Collections.Generic;

namespace Plk.Blazor.DragDrop.DropActions
{
    public interface IDropAction<TItem>
    {
        void Execute(TItem activeItem, IList<TItem> sourceItems, IList<TItem> targetItems, int? targetItemIndex);
    }
}
