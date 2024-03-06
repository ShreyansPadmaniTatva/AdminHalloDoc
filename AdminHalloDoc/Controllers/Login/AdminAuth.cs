using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AdminHalloDoc.Controllers.Login
{
    public class AdminAuth : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

       

        public AdminAuth(string role) { 
            _role = role;
        }

        #region CheckAccessOrNot
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var admin = SessionUtils.GetLogginUser(filterContext.HttpContext.Session);

            var rd = filterContext.RouteData;
            string currentAction = rd.Values["action"].ToString();
            string currentController = rd.Values["controller"].ToString();

            //string currentArea = rd.DataTokens["area"].ToString();

            if (admin == null)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin");
                return;
            }

            if(!string.IsNullOrEmpty(_role))
            {
                if (!(admin.Role == _role))
                {
                    filterContext.Result = new RedirectResult("~/AdminLogin/AccessDenied");
                }
            }
        }
        #endregion

    }
}
