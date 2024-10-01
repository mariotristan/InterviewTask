using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

public class CustomAuthorizationHandler : AuthorizationHandler<CustomRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
    {
        var user = context.User;

        // Check if all required claims are present
        var objectIdClaim = user.FindFirst("objectId");
        var rolesClaim = user.FindFirst("roles");
        var idpClaim = user.FindFirst("idp");

        if (objectIdClaim == null || rolesClaim == null || idpClaim == null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // Check if roles contain the authorized role
        var roles = rolesClaim.Value.Split(',');
        if (!roles.Contains("AuthorizedRoleName"))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // Check if idp claim value matches the authorized idp name
        if (idpClaim.Value != "AuthorizedIdpName")
        {
            context.Fail();
            return Task.CompletedTask;
        }

        // Authorization successful
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

public class CustomRequirement : IAuthorizationRequirement
{
    // Custom requirement properties can be added here if needed
}
