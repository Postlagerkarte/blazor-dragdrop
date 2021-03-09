using System;
using System.Collections.Generic;

namespace Plk.Blazor.DragDrop.DropActions
{
    public interface IDropAction<TItem>
    {
        Func<DropActionContext<TItem>, bool> IsSuitable { get; set; }

        void Execute(DropActionContext<TItem> context);
    }
}
