using AgrifoodManagement.Business.CommandHandlers.Account;

namespace AgrifoodManagement.Web.Services
{
    public static class RegisterMediatR
    {
        public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
        {
            // Register the assembly containing handlers for your application
            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>()
                .RegisterServicesFromAssemblyContaining<UploadUserPhotoCommandHandler>());
            return services;
        }
    }
}
