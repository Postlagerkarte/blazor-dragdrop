using Microsoft.AspNetCore.Components;

namespace Plk.Blazor.DragDrop.Demo.Data
{
    public abstract class Field
    {
        public string Title { get; set; }

        public abstract RenderFragment RenderFieldEdit();

        public ComponentBase ParentComponent { get; set; }
    }
}
