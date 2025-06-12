using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Unbiased.Shared.Dtos.Concrete.Configurations;
using Unbiased.Shared.Extensions.Concrete.Helpers;

namespace Unbiased.Shared.Extensions.Concrete.Extensions
{
    /// <summary>
    /// Provides extension methods for adding custom token authentication to the service collection.
    /// </summary>
    public static class CustomTokenAuth
    {
        /// <summary>
        /// Adds custom token authentication to the service collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="tokenOptions"></param>
        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Dashboard Add", policy => policy.RequireClaim("permissions", "Dashboard Add"));
                options.AddPolicy("Dashboard Delete", policy => policy.RequireClaim("permissions", "Dashboard Delete"));
                options.AddPolicy("Dashboard Update", policy => policy.RequireClaim("permissions", "Dashboard Update"));
                options.AddPolicy("Dashboard Get", policy => policy.RequireClaim("permissions", "Dashboard Get"));

                options.AddPolicy("Blog Posts Add", policy => policy.RequireClaim("permissions", "Blog Posts Add"));
                options.AddPolicy("Blog Posts Delete", policy => policy.RequireClaim("permissions", "Blog Posts Delete"));
                options.AddPolicy("Blog Posts Update", policy => policy.RequireClaim("permissions", "Blog Posts Update"));
                options.AddPolicy("Blog Posts Get", policy => policy.RequireClaim("permissions", "Blog Posts Get"));

                options.AddPolicy("News Add", policy => policy.RequireClaim("permissions", "News Add"));
                options.AddPolicy("News Delete", policy => policy.RequireClaim("permissions", "News Delete"));
                options.AddPolicy("News Update", policy => policy.RequireClaim("permissions", "News Update"));
                options.AddPolicy("News Get", policy => policy.RequireClaim("permissions", "News Get"));

                options.AddPolicy("Access Control Add", policy => policy.RequireClaim("permissions", "Access Control Add"));
                options.AddPolicy("Access Control Delete", policy => policy.RequireClaim("permissions", "Access Control Delete"));
                options.AddPolicy("Access Control Update", policy => policy.RequireClaim("permissions", "Access Control Update"));
                options.AddPolicy("Access Control Get", policy => policy.RequireClaim("permissions", "Access Control Get"));

                options.AddPolicy("Customer Delete", policy => policy.RequireClaim("permissions", "Customer Delete"));
                options.AddPolicy("Customer Get", policy => policy.RequireClaim("permissions", "Customer Get"));

                options.AddPolicy("Profile Add", policy => policy.RequireClaim("permissions", "Profile Add"));
                options.AddPolicy("Profile Delete", policy => policy.RequireClaim("permissions", "Profile Delete"));
                options.AddPolicy("Profile Update", policy => policy.RequireClaim("permissions", "Profile Update"));
                options.AddPolicy("Profile Get", policy => policy.RequireClaim("permissions", "Profile Get"));

                options.AddPolicy("Settings Add", policy => policy.RequireClaim("permissions", "Settings Add"));
                options.AddPolicy("Settings Delete", policy => policy.RequireClaim("permissions", "Settings Delete"));
                options.AddPolicy("Settings Update", policy => policy.RequireClaim("permissions", "Settings Update"));
                options.AddPolicy("Settings Get", policy => policy.RequireClaim("permissions", "Settings Get"));

                options.AddPolicy("Engine Management Add", policy => policy.RequireClaim("permissions", "Engine Management Add"));
                options.AddPolicy("Engine Management Delete", policy => policy.RequireClaim("permissions", "Engine Management Delete"));
                options.AddPolicy("Engine Management Update", policy => policy.RequireClaim("permissions", "Engine Management Update"));
                options.AddPolicy("Engine Management Get", policy => policy.RequireClaim("permissions", "Engine Management Get"));
                options.AddPolicy("Engine Management Generate Content", policy => policy.RequireClaim("permissions", "Engine Management Generate Content"));

                options.AddPolicy("Content Management Add", policy => policy.RequireClaim("permissions", "Content Management Add"));
                options.AddPolicy("Content Management Delete", policy => policy.RequireClaim("permissions", "Content Management Delete"));
                options.AddPolicy("Content Management Update", policy => policy.RequireClaim("permissions", "Content Management Update"));
                options.AddPolicy("Content Management Get", policy => policy.RequireClaim("permissions", "Content Management Get"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience[0],
                    IssuerSigningKey = SigningSecurityKey.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
