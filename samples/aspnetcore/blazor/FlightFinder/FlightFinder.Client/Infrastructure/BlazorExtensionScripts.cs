using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightFinder.Client.Infrastructure
{
    public class BlazorExtensionScripts : Microsoft.AspNetCore.Blazor.Components.BlazorComponent
    {
        protected override void BuildRenderTree(Microsoft.AspNetCore.Blazor.RenderTree.RenderTreeBuilder builder)
        {
            builder.OpenElement(0, "script");
            builder.AddContent(1, "Blazor.registerFunction('Alert', (message) => { alert(message); });");
            builder.CloseElement();
        }
    }
}
