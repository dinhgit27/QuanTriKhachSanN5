using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace QuanTriKhachSanN5.Attributes
{
    public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public PermissionAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (role == null || !_roles.Contains(role))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}