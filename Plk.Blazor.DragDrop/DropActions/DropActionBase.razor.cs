using System;
using Microsoft.AspNetCore.Components;

namespace Plk.Blazor.DragDrop.DropActions
{
    public abstract partial class DropActionBase<TItem> : IDropAction<TItem>
    {
        [Parameter]
        public Func<DropActionContext<TItem>, bool> IsSuitable { get; set; } = (i) => true;

        public abstract void Execute(DropActionContext<TItem> context);
    }
}
