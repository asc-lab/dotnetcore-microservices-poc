using AgentPortalUi.BlazorWasm.Contracts;
using AgentPortalUi.BlazorWasm.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Net;

namespace AgentPortalUi.BlazorWasm
{
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public UserService UserService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            if (authorize && !UserService.IsAuthenticated)
            {
                //var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                //NavigationManager.NavigateTo($"login?returnUrl={returnUrl}");
                NavigationManager.NavigateTo($"login");
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}
