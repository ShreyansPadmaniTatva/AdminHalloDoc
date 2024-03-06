﻿using AdminHalloDoc.Repositories.Admin.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AdminHalloDoc.Controllers.Login
{
    [AttributeUsage(AttributeTargets.All)]
    public class AdminAuth : Attribute, IAuthorizationFilter
    {
        private readonly string _role;
       

        public AdminAuth(string role) { 
            _role = role;
           
        }

        #region CheckAccessOrNot
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var jwtservice = filterContext.HttpContext.RequestServices.GetService<IJwtService>();

            if(jwtservice == null)
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

            if (string.IsNullOrWhiteSpace(_role) || roles.Value != _role)
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