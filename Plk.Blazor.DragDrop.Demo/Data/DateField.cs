using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Linq.Expressions;

namespace Plk.Blazor.DragDrop.Demo.Data
{
    public class DateField : Field
    {
        public DateTimeOffset Value { get; set; }

        public override RenderFragment RenderFieldEdit()
        {
            return builder =>
            {
                builder.OpenComponent<InputDate<DateTimeOffset>>(0);

                builder.AddAttribute(1, "Value", Value);

                builder.AddAttribute(2, "ValueChanged", EventCallback.Factory.Create(this.ParentComponent, (DateTimeOffset val) => Value = val));

                Expression<Func<DateTimeOffset>> valExpr = () => Value;

                builder.AddAttribute(3, "ValueExpression", valExpr);

                builder.CloseComponent();
            };
        }
    }
}
