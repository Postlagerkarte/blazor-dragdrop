using System.Collections.Generic;

namespace Plk.Blazor.DragDrop.DropActions
{
    public class MoveDropAction<TItem> : IDropAction<TItem>
    {
        public void Execute(TItem activeItem, IList<TItem> sourceItems, IList<TItem> targetItems, int? targetItemIndex)
        {
            sourceItems.Remove(activeItem);

            if (targetItemIndex.HasValue)
            {
                targetItems.Insert(targetItemIndex.Value, activeItem);
            }
            else
            {
                targetItems.Add(activeItem);
            }
        }
    }
}
