using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RealtimeGames.Server.Areas.Identity.Models;

namespace RealtimeGames.Server.Filters
{
    public class RedirectAuthenticatedUserFilter : IPageFilter
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RedirectAuthenticatedUserFilter(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (_signInManager.IsSignedIn(((PageModel)context.HandlerInstance).User))
            {
                context.Result = new RedirectResult("/");
            }
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {

        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {

        }
    }
}
