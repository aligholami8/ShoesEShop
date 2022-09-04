using System.Net;
using System.Security.Claims;
using System.Text;
using Common;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PoolManagement;
namespace Service.Middlewares;

public static class CustomIdentity
{
    public static void AddCustomIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequiredLength = 4;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();
    }

    public static void AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            var secretKey = Encoding.UTF8.GetBytes("wJ4PT85k3Z3KYL9Drk");
            var encryptionKey = Encoding.UTF8.GetBytes("xxV93VD3XF1FTzOp");

            var validationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                RequireExpirationTime = true,
                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey),
                ValidAudience = "shoeeshop",
                ValidIssuer = "shoeeshop",
            };


            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception != null)
                        throw new AppException(ApiResultStatusCode.UnAuthorized,
                            "احراز هویت ناموفق", HttpStatusCode.Unauthorized, context.Exception, null);
                    return Task.CompletedTask;
                },
                OnTokenValidated = async context =>
                {
                    var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                    var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;
                    if (claimsIdentity?.Claims?.Any() != true)
                        context.Fail("توکن معتبر نمیباشد");
                    var securityStamp = claimsIdentity.FindFirstValue(new ClaimsIdentityOptions().SecurityStampClaimType);
                    if (!securityStamp.HasValue())
                        context.Fail("توکن معتبر نمیباشد");
                    var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                    if (validatedUser == null)
                        context.Fail("توکن معتبر نمیباشد");
                },
                OnChallenge = context =>
                {
                    if (context.AuthenticateFailure != null)
                        throw new AppException(ApiResultStatusCode.UnAuthorized, "احراز هویت ناموفق بود",
                            HttpStatusCode.Unauthorized, context.AuthenticateFailure, null);
                    throw new AppException(ApiResultStatusCode.UnAuthorized,
                        "شما دسترسی به این اکشن ندارید", HttpStatusCode.Unauthorized);
                }
            };
        });
    }
}