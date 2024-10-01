using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Xunit;

public class CustomAuthorizationHandlerTests
{
    [Fact]
    public async Task Should_Fail_If_Claims_Are_Missing()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        var context = new AuthorizationHandlerContext(new[] { new CustomRequirement() }, user, null);
        var handler = new CustomAuthorizationHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task Should_Fail_If_Roles_Do_Not_Contain_AuthorizedRoleName()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("objectId", "123"),
            new Claim("roles", "User,Admin"),
            new Claim("idp", "AuthorizedIdpName")
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var context = new AuthorizationHandlerContext(new[] { new CustomRequirement() }, user, null);
        var handler = new CustomAuthorizationHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task Should_Fail_If_Idp_Claim_Value_Is_Incorrect()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("objectId", "123"),
            new Claim("roles", "AuthorizedRoleName"),
            new Claim("idp", "IncorrectIdpName")
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var context = new AuthorizationHandlerContext(new[] { new CustomRequirement() }, user, null);
        var handler = new CustomAuthorizationHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task Should_Succeed_If_All_Conditions_Are_Met()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("objectId", "123"),
            new Claim("roles", "AuthorizedRoleName"),
            new Claim("idp", "AuthorizedIdpName")
        };
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
        var context = new AuthorizationHandlerContext(new[] { new CustomRequirement() }, user, null);
        var handler = new CustomAuthorizationHandler();

        // Act
        await handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }
}
