
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace RNGService.Services
{

    public class AuthorizationHandler : AuthorizationHandler<CustomAuthorizationRequirement>
    {
        private readonly IMemoryCache cache;
        private const string AuthorizedRoleName = "Contributor";
        private const string AuthorizedIdPName = "idpInterview.com";

        public AuthorizationHandler(IMemoryCache cache)
        {
            this.cache = cache;
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
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomAuthorizationRequirement requirement)
        {
            return;
        }
    }
}