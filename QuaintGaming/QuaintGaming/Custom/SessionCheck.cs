
using System.Web;
using System.Web.Mvc;

namespace QuaintGaming.Custom
{
    public class SessionCheck : ActionFilterAttribute
    {
        //Starting point for implementing session through filter attributes.
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session["Username"] == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login", false);
            }
            base.OnActionExecuting(filterContext);
        }
    }
}