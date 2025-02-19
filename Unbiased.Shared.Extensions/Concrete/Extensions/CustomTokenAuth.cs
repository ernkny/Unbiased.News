using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Unbiased.Shared.Dtos.Concrete.Configurations;
using Unbiased.Shared.Extensions.Concrete.Helpers;

namespace Unbiased.Shared.Extensions.Concrete.Extensions
{
    public static class CustomTokenAuth
    {

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

                options.AddPolicy("Media Library Add", policy => policy.RequireClaim("permissions", "Media Library Add"));
                options.AddPolicy("Media Library Delete", policy => policy.RequireClaim("permissions", "Media Library Delete"));
                options.AddPolicy("Media Library Update", policy => policy.RequireClaim("permissions", "Media Library Update"));
                options.AddPolicy("Media Library Get", policy => policy.RequireClaim("permissions", "Media Library Get"));
                 
                options.AddPolicy("Profile Add", policy => policy.RequireClaim("permissions", "Profile Add"));
                options.AddPolicy("Profile Delete", policy => policy.RequireClaim("permissions", "Profile Delete"));
                options.AddPolicy("Profile Update", policy => policy.RequireClaim("permissions", "Profile Update"));
                options.AddPolicy("Profile Get", policy => policy.RequireClaim("permissions", "Profile Get"));

                options.AddPolicy("Settings Add", policy => policy.RequireClaim("permissions", "Settings Add"));
                options.AddPolicy("Settings Delete", policy => policy.RequireClaim("permissions", "Settings Delete"));
                options.AddPolicy("Settings Update", policy => policy.RequireClaim("permissions", "Settings Update"));
                options.AddPolicy("Settings Get", policy => policy.RequireClaim("permissions", "Settings Get"));
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
