using System;
using System.Collections.Generic;

namespace Plk.Blazor.DragDrop.DropActions
{
    public class CopyDropAction<TItem> : IDropAction<TItem>
    {
        public Func<TItem, TItem> CopyItem { get; set; }

        public void Execute(TItem activeItem, IList<TItem> sourceItems, IList<TItem> targetItems, int? targetItemIndex)
        {  
            TItem copiedItem = CopyItem != null ? CopyItem(activeItem) : activeItem;

            if (copiedItem == null)
                return;

            if (targetItemIndex.HasValue)
            {
                targetItems.Insert(targetItemIndex.Value, copiedItem);
            }
            else
            {
                targetItems.Add(copiedItem);
            }
        }
    }
}
