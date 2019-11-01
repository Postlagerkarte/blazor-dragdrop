using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.DragDrop.Core
{
    public class DropzoneOptions
    {
        public int? MaxItems { get; set; }

        public bool AllowSwap { get; set; }

        public Func<dynamic, bool> Accepts { get; set; }

        
    }
}
