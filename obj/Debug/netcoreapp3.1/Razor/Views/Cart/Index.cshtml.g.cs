#pragma checksum "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7e5ff7ead02fc4cf46c653fe6f461fbab0e5f38a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Cart_Index), @"mvc.1.0.view", @"/Views/Cart/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/_ViewImports.cshtml"
using ClaveSol;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/_ViewImports.cshtml"
using ClaveSol.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7e5ff7ead02fc4cf46c653fe6f461fbab0e5f38a", @"/Views/Cart/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"a93bae2403598b9867963e3ffd85df01f6958904", @"/Views/_ViewImports.cshtml")]
    public class Views_Cart_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<ClaveSol.Models.LineOrder>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
  
    ViewData["Title"] = "Carrito";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Carrito</h1>\r\n\r\n");
            WriteLiteral("<table class=\"table table-bordered table-hover table-responsive table-striped\">\r\n    <thead>\r\n        <tr class=\"text-center\">\r\n            <th>\r\n                ");
#nullable restore
#line 16 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>\r\n                ");
#nullable restore
#line 19 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
           Write(Html.DisplayNameFor(model => model.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </th>\r\n            <th>Eliminar</th>\r\n        </tr>\r\n    </thead>\r\n    <tbody>\r\n");
#nullable restore
#line 25 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
 foreach (var item in Model) {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr class=\"text-center\">\r\n            <td class=\"lineIds\">\r\n                ");
#nullable restore
#line 28 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.Id));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 31 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
           Write(Html.DisplayFor(modelItem => item.Name));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n               <a class=\"deleteLineOrderlink\">Eliminar</a>  \r\n            </td>\r\n");
            WriteLiteral("        </tr>\r\n");
#nullable restore
#line 42 "/home/dlag/DAW/PROYECTO-FINAL-DAW/ClaveSol/Views/Cart/Index.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("    </tbody>\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<ClaveSol.Models.LineOrder>> Html { get; private set; }
    }
}
#pragma warning restore 1591