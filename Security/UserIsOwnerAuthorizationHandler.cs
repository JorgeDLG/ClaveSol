using Microsoft.VisualBasic;
using ClaveSol.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace ClaveSol.Security
{
    public class UserIsOwnerAuthorizationHandler 
        : AuthorizationHandler<OperationAuthorizationRequirement, User>
    {
        UserManager<appIdentityUser> _userManager;

        public UserIsOwnerAuthorizationHandler(UserManager<appIdentityUser> 
            userManager)
        {
            _userManager = userManager;
        }

        protected override Task
            HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement,
                User resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            //% CONSTANTS CUSTOM CLASS CREATED here: https://docs.microsoft.com/es-es/aspnet/core/security/authorization/secure-data?view=aspnetcore-3.1#review-the-contact-operations-requirements-class

            // If not asking for CRUD permission, return.
            if (requirement.Name != Constants.CreateOperationName &&
                requirement.Name != Constants.ReadOperationName   &&
                requirement.Name != Constants.UpdateOperationName &&
                requirement.Name != Constants.DeleteOperationName )
            {
                return Task.CompletedTask;
            }

            if (resource.OwnerID == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}