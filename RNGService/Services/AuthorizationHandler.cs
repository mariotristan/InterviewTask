using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

public class CustomAuthorizationHandler : AuthorizationHandler<CustomRequirement>
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public CustomAuthorizationHandler(IMemoryCache cache)
    {
        _cache = cache;
    }
    /// <summary>
    /// Handler must verify if the claims identity contains claims: objectId, roles and idp.
    /// If does not contain all claims -> authorization fails
    /// if Roles don't contain "AuthorizedRoleName", authorization fails
    /// if idp claim value is different than "AuthorizedIdpName", authorization fails
    /// Otherwise authentication is successfull
    /// 
    /// Unit tests must cover the implementation (functionality, corner cases etc)
    /// </summary>
    /// <param name="context"></param>
    /// <param name="requirement"></param>
    /// <returns></returns>

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
    {
        var user = context.User;
        var cacheKey = GetCacheKey(user);

        if (_cache.TryGetValue(cacheKey, out bool isAuthorized))
        {
            if (isAuthorized)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

        // Check if all required claims are present
        var objectIdClaim = user.FindFirst("objectId");
        var rolesClaim = user.FindFirst("roles");
        var idpClaim = user.FindFirst("idp");

        if (objectIdClaim == null || rolesClaim == null || idpClaim == null)
        {
            _cache.Set(cacheKey, false, CacheDuration);
            context.Fail();
            return Task.CompletedTask;
        }

        // Check if roles contain the authorized role
        var roles = rolesClaim.Value.Split(',');
        if (!roles.Contains("AuthorizedRoleName"))
        {
            _cache.Set(cacheKey, false, CacheDuration);
            context.Fail();
            return Task.CompletedTask;
        }

        // Check if idp claim value matches the authorized idp name
        if (idpClaim.Value != "AuthorizedIdpName")
        {
            _cache.Set(cacheKey, false, CacheDuration);
            context.Fail();
            return Task.CompletedTask;
        }

        // Authorization successful
        _cache.Set(cacheKey, true, CacheDuration);
        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private string GetCacheKey(ClaimsPrincipal user)
    {
        var objectId = user.FindFirst("objectId")?.Value ?? string.Empty;
        var roles = user.FindFirst("roles")?.Value ?? string.Empty;
        var idp = user.FindFirst("idp")?.Value ?? string.Empty;
        return $"{objectId}-{roles}-{idp}";
    }
}

public class CustomRequirement : IAuthorizationRequirement
{
    // Custom requirement properties can be added here if needed
}
