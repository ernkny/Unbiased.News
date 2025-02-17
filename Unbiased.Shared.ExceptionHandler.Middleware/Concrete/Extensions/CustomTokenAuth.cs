using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unbiased.Shared.Dtos.Concrete.Configurations;
using Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Helpers;

namespace Unbiased.Shared.ExceptionHandler.Middleware.Concrete.Extensions
{
    public static class CustomTokenAuth
    {

        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("DashboardAdd", policy => policy.RequireClaim("permissions", "Dashboard Add"));
                options.AddPolicy("DashboardDelete", policy => policy.RequireClaim("permissions", "Dashboard Delete"));
                options.AddPolicy("DashboardUpdate", policy => policy.RequireClaim("permissions", "Dashboard Update"));
                options.AddPolicy("DashboardGet", policy => policy.RequireClaim("permissions", "Dashboard Get"));

                options.AddPolicy("BlogPostsAdd", policy => policy.RequireClaim("permissions", "Blog Posts Add"));
                options.AddPolicy("BlogPostsDelete", policy => policy.RequireClaim("permissions", "Blog Posts Delete"));
                options.AddPolicy("BlogPostsUpdate", policy => policy.RequireClaim("permissions", "Blog Posts Update"));
                options.AddPolicy("BlogPostsGet", policy => policy.RequireClaim("permissions", "Blog Posts Get"));

                options.AddPolicy("NewsAdd", policy => policy.RequireClaim("permissions", "News Add"));
                options.AddPolicy("NewsDelete", policy => policy.RequireClaim("permissions", "News Delete"));
                options.AddPolicy("NewsUpdate", policy => policy.RequireClaim("permissions", "News Update"));
                options.AddPolicy("News Get", policy => policy.RequireClaim("permissions", "News Get"));

                options.AddPolicy("AccessControlAdd", policy => policy.RequireClaim("permissions", "Access Control Add"));
                options.AddPolicy("AccessControlDelete", policy => policy.RequireClaim("permissions", "Access Control Delete"));
                options.AddPolicy("AccessControlUpdate", policy => policy.RequireClaim("permissions", "Access Control Update"));
                options.AddPolicy("AccessControlGet", policy => policy.RequireClaim("permissions", "Access Control Get"));

                options.AddPolicy("MediaLibraryAdd", policy => policy.RequireClaim("permissions", "Media Library Add"));
                options.AddPolicy("MediaLibraryDelete", policy => policy.RequireClaim("permissions", "Media Library Delete"));
                options.AddPolicy("MediaLibraryUpdate", policy => policy.RequireClaim("permissions", "Media Library Update"));
                options.AddPolicy("MediaLibraryGet", policy => policy.RequireClaim("permissions", "Media Library Get"));

                options.AddPolicy("ProfileAdd", policy => policy.RequireClaim("permissions", "Profile Add"));
                options.AddPolicy("ProfileDelete", policy => policy.RequireClaim("permissions", "Profile Delete"));
                options.AddPolicy("ProfileUpdate", policy => policy.RequireClaim("permissions", "Profile Update"));
                options.AddPolicy("ProfileGet", policy => policy.RequireClaim("permissions", "Profile Get"));

                options.AddPolicy("SettingsAdd", policy => policy.RequireClaim("permissions", "Settings Add"));
                options.AddPolicy("SettingsDelete", policy => policy.RequireClaim("permissions", "Settings Delete"));
                options.AddPolicy("SettingsUpdate", policy => policy.RequireClaim("permissions", "Settings Update"));
                options.AddPolicy("SettingsGet", policy => policy.RequireClaim("permissions", "Settings Get"));
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
