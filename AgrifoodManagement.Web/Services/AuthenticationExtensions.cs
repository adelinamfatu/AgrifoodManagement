using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AgrifoodManagement.Web.Services
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSecretKey = configuration["Jwt:SecretKey"];
            var key = Encoding.ASCII.GetBytes(jwtSecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("AuthToken"))
                        {
                            context.Token = context.Request.Cookies["AuthToken"];
                        }
                        return Task.CompletedTask;
                    },

                    OnChallenge = ctx =>
                    {
                        ctx.HandleResponse();

                        ctx.Response.StatusCode = StatusCodes.Status302Found;
                        ctx.Response.Headers.Location = "/Account/Auth";
                        return Task.CompletedTask;
                    },

                    OnForbidden = context =>
                    {
                        var token = context.Request.Cookies["AuthToken"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "Role")?.Value;

                            if (roleClaim == "Seller")
                            {
                                context.Response.StatusCode = StatusCodes.Status302Found;
                                context.Response.Headers.Location = "/Producer/Announcements";
                                return Task.CompletedTask;
                            }

                            if (roleClaim == "Buyer")
                            {
                                context.Response.StatusCode = StatusCodes.Status302Found;
                                context.Response.Headers.Location = "/Consumer/Home";
                                return Task.CompletedTask;
                            }
                        }

                        // Fallback if no token or unknown role
                        context.Response.StatusCode = StatusCodes.Status302Found;
                        context.Response.Headers.Location = "/Account/Auth";
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"])),
                    RoleClaimType = "Role"
                };
            });

            return services;
        }
    }
}
