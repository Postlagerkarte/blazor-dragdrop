using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Plk.Blazor.DragDrop.Demo.Data
{
    public class TextField : Field
    {

        public string Value { get; set; }

        public override RenderFragment RenderFieldEdit()
        {
            return builder =>
            {
                builder.OpenComponent<InputText>(0);

                builder.AddAttribute(1, "Value", Value);

                builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create(this.ParentComponent, (string val) => Value = val));

                Expression<Func<string>> valExpr = () => Value;

                builder.AddAttribute(3, "ValueExpression", valExpr);

                builder.CloseComponent();
            };
        }
    }
}
