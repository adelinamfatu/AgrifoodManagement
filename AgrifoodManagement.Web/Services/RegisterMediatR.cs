using AgrifoodManagement.Business.CommandHandlers.Account;
using AgrifoodManagement.Business.CommandHandlers.Announcement;
using AgrifoodManagement.Business.CommandHandlers.Forum;

namespace AgrifoodManagement.Web.Services
{
    public static class RegisterMediatR
    {
        public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
        {
            // Register the assembly containing handlers
            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>()
                .RegisterServicesFromAssemblyContaining<UploadProductPhotoCommandHandler>());

            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<CreateProductCommandHandler>());

            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<CreateForumThreadCommandHandler>());

            return services;
        }
    }
}
