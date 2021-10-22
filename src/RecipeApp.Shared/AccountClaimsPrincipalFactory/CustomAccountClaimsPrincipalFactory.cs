using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Microsoft.Extensions.Logging;

namespace RecipeApp.Shared.AccountClaimsPrincipalFactory
{
    /// <summary>
    /// https://medium.com/@marcodesanctis2/role-based-security-with-blazor-and-identity-server-4-aba12da70049
    /// </summary>
    public class CustomAccountClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
    {
        protected readonly ILogger<CustomAccountClaimsPrincipalFactory> _logger;

        public CustomAccountClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor, ILogger<CustomAccountClaimsPrincipalFactory> logger) : base(accessor) =>
            _logger = logger;

        public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
        {
            var user = await base.CreateUserAsync(account, options);

            if (user.Identity.IsAuthenticated)
            {
                var identity = (ClaimsIdentity)user.Identity;
                var roleClaims = identity.FindAll(identity.RoleClaimType);

                if (roleClaims?.Any() == true)
                {
                    foreach (var existingClaim in roleClaims)
                    {
                        identity.RemoveClaim(existingClaim);
                    }

                    var rolesElem = account.AdditionalProperties[identity.RoleClaimType];

                    if (rolesElem is JsonElement roles)
                    {
                        if (roles.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var role in roles.EnumerateArray())
                            {
                                identity.AddClaim(new Claim(options.RoleClaim, role.GetString()));
                            }
                        }
                        else
                        {
                            identity.AddClaim(new Claim(options.RoleClaim, roles.GetString()));
                        }
                    }
                }

                _logger.LogInformation("var accessTokenResult = await TokenProvider.RequestAccessToken();");
                var accessTokenResult = await TokenProvider.RequestAccessToken();
                _logger.LogInformation("accessTokenResult.TryGetToken(out AccessToken accessToken);");
                accessTokenResult.TryGetToken(out AccessToken accessToken);
                _logger.LogInformation($"var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken({accessToken?.Value});");
                if (string.IsNullOrWhiteSpace(accessToken?.Value))
                    return user;
                try
                {
                    var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken.Value);
                    _logger.LogInformation("Processing Jwt Role Claims");
                    foreach (var jwtRoleClaim in jwtSecurityToken.Claims.Where(role => role.Type.Equals(options.RoleClaim)))
                    {
                        foreach (var value in jwtRoleClaim.Value.Split(','))
                            identity.AddClaim(new Claim(options.RoleClaim, value));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error reading the JWT Token!", null);
                }
            }

            return user;
        }
    }
}

