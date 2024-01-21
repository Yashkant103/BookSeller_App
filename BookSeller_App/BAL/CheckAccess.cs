using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookSeller_App.BAL
{
    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var rd = filterContext.RouteData;
            _ = rd.Values["action"].ToString();
            _ = rd.Values["controller"].ToString();


            if (filterContext.HttpContext.Session.GetString("UserName") == null)
            {
                filterContext.HttpContext.Session.Clear();
                filterContext.Result = new RedirectResult("~/Login");
            }
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.CacheControl = "no-cache, no-store, must-revalidate";
            filterContext.HttpContext.Response.Headers.Expires = "-1";
            filterContext.HttpContext.Response.Headers.Pragma = "no-cache";

            base.OnResultExecuting(filterContext);
        }
    }
}
