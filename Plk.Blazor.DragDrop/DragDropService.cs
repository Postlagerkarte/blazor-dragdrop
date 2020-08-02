using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Plk.Blazor.DragDrop
{
    internal class DragDropService<T>
    {
        public T ActiveItem { get; set; }
        public List<T> Items { get; set; }
    }
}
