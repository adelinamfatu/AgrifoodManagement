using AgrifoodManagement.Business.CommandHandlers.Account;
using AgrifoodManagement.Business.CommandHandlers.Announcement;
using AgrifoodManagement.Business.CommandHandlers.Forum;
using AgrifoodManagement.Business.Commands.Order;
using AgrifoodManagement.Business.Commands.Product;

namespace AgrifoodManagement.Web.Services
{
    public static class RegisterMediatR
    {
        public static IServiceCollection AddApplicationMediatR(this IServiceCollection services)
        {
            // Register the assembly containing handlers
            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<RegisterUserCommandHandler>()
                .RegisterServicesFromAssemblyContaining<LoginUserCommandHandler>()
                .RegisterServicesFromAssemblyContaining<UploadUserPhotoCommandHandler>());

            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<CreateProductCommandHandler>()
                .RegisterServicesFromAssemblyContaining<PromoteProductCommandHandler>()
                .RegisterServicesFromAssemblyContaining<UpdateProductCommandHandler>()
                .RegisterServicesFromAssemblyContaining<UpdateProductStatusCommandHandler>()
                .RegisterServicesFromAssemblyContaining<DeleteProductPhotosCommandHandler>());

            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<AddCommentCommandHandler>()
                .RegisterServicesFromAssemblyContaining<CreateForumThreadCommandHandler>());

            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<AddProductToCartCommand>()
                .RegisterServicesFromAssemblyContaining<RemoveCartItemCommand>()
                .RegisterServicesFromAssemblyContaining<UpdateCartItemQuantityCommand>()
                .RegisterServicesFromAssemblyContaining<ConfirmOrderCommand>());

            services.AddMediatR(cfg => cfg
                .RegisterServicesFromAssemblyContaining<UpdateProductStockCommand>()
                .RegisterServicesFromAssemblyContaining<UpdateOrderStatusCommand>());

            return services;
        }
    }
}
