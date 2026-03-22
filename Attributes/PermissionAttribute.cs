using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace QuanTriKhachSanN5.Attributes
{
public class PermissionAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _permissions;

        public PermissionAttribute(params string[] permissions)
        {
            _permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if user has ANY required permission claim from JWT
            var userPermissions = context.HttpContext.User.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToList();

            bool hasPermission = userPermissions.Any(p => _permissions.Contains(p));

            if (!hasPermission)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}