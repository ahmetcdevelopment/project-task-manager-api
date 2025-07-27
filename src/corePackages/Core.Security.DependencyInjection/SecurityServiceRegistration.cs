using Microsoft.Extensions.DependencyInjection;
using Core.Security.EmailAuthenticator;
using Core.Security.JWT;
using Core.Security.OtpAuthenticator;
using Core.Security.OtpAuthenticator.OtpNet;
using Microsoft.Extensions.Configuration;

namespace Core.Security.DependencyInjection;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices<TUserId, TOperationClaimId>(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        services.AddScoped<
            ITokenHelper<TUserId, TOperationClaimId>,
            JwtHelper<TUserId, TOperationClaimId>
        >(_ => new JwtHelper<TUserId, TOperationClaimId>(configuration));
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticatorHelper, OtpNetOtpAuthenticatorHelper>();

        return services;
    }
}
