using AdminHalloDoc.Models.CV;
using AdminHalloDoc.Repositories.Admin.Repository;
using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static AdminHalloDoc.Entities.ViewModel.Constant;

namespace AdminHalloDoc.Controllers.Login
{
    [AttributeUsage(AttributeTargets.All)]
    public class AdminAuth : Attribute, IAuthorizationFilter
    {
        private readonly List<string> _role;
       

        public AdminAuth(string role = "") {
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

            if(toket == null || !jwtservice.ValidateToken(toket,out JwtSecurityToken jwtSecurityTokenHandler))
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

            if (Staticmenu == null)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin/AccessDenied");
            }

            if (!flage)
            {
                filterContext.Result = new RedirectResult("~/AdminLogin/AccessDenied");
            }
            else
            {

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
