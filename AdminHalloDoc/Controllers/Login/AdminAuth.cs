using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static AdminHalloDoc.Repositories.Admin.Repository.LoginRepository;

namespace AdminHalloDoc.Controllers.Login
{
    [AttributeUsage(AttributeTargets.All)]
    public class AdminAuth : Attribute, IAuthorizationFilter
    {
        private readonly List<string> _role;


        public AdminAuth(string role = "")
        {
            _role = role.Split(',').ToList();

        }

        #region CheckAccessOrNot
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var jwtservice = filterContext.HttpContext.RequestServices.GetService<IJwtService>();
            var loginservice = filterContext.HttpContext.RequestServices.GetService<ILoginRepository>();


            if (jwtservice == null)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin");
                return;
            }

            var request = filterContext.HttpContext.Request;
            var toket = request.Cookies["jwt"];
            if (toket == null)
            {
                if (filterContext.HttpContext.Request.Headers.TryGetValue("X-Requested-With", out var requestedWith) && requestedWith.FirstOrDefault() == "XMLHttpRequest")
                {
                    filterContext.Result = new BadRequestObjectResult(new ProblemDetails
                    {
                        Status = 401
                    });
                    return;
                }

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                return;
            }

            if (toket == null || !jwtservice.ValidateToken(toket, out JwtSecurityToken jwtSecurityTokenHandler))
            {
                filterContext.Result = new RedirectResult("~/AdminLogin");
                return;
            }

            var roles = jwtSecurityTokenHandler.Claims.FirstOrDefault(claiim => claiim.Type == ClaimTypes.Role);

            if (roles == null)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin");
                return;
            }


            bool flage = false;
            foreach (var role in _role)
            {

                if (string.IsNullOrWhiteSpace(role) || roles.Value != role)
                {

                    flage = false;
                }
                else
                {
                    flage = true;
                    break;
                }
            }

            var Path = filterContext.HttpContext.Request.Path;

            List<MenuItem> Staticmenu = loginservice.SetMenu(CV.RoleId());


            bool isPathAvailable = Staticmenu.Any(item =>
                item.Url.Equals(Path, StringComparison.OrdinalIgnoreCase) ||
                item.ContollerAction.Equals(Path, StringComparison.OrdinalIgnoreCase)
                ||
                (item.Submenu != null && item.Submenu.Any(submenu =>
                    submenu.Url.Equals(Path, StringComparison.OrdinalIgnoreCase) || submenu.ContollerAction.Equals(Path, StringComparison.OrdinalIgnoreCase)
                    )));


            //bool isPathAvailable = Staticmenu.Any(item => item.Url.Equals(Path, StringComparison.OrdinalIgnoreCase) || item.ContollerAction.Equals(Path, StringComparison.OrdinalIgnoreCase));


            if ((Staticmenu == null || !flage || !isPathAvailable) && roles.Value != "Patient")
            {
                filterContext.Result = new RedirectResult("~/AdminLogin/AccessDenied");
            }
            else if (!flage)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin/AccessDenied");
            }

            //var admin = SessionUtils.GetLogginUser(filterContext.HttpContext.Session);

            //var rd = filterContext.RouteData;
            //string currentAction = rd.Values["action"].ToString();
            //string currentController = rd.Values["controller"].ToString();

            ////string currentArea = rd.DataTokens["area"].ToString();

            //if (admin == null)
            //{
            //    filterContext.Result = new RedirectResult("~/AdminLogin");
            //    return;
            //}

            //if(!string.IsNullOrEmpty(_role))
            //{
            //    if (!(admin.Role == _role))
            //    {
            //        filterContext.Result = new RedirectResult("~/AdminLogin/AccessDenied");
            //    }
            //}
        }
        #endregion

    }
}
