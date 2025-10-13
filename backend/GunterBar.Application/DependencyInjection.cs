using Microsoft.Extensions.DependencyInjection;
using GunterBar.Application.Interfaces;
using GunterBar.Application.Services;
using GunterBar.Application.UseCases;

namespace GunterBar.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrar los servicios de la capa de aplicación
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDrinkService, DrinkService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();

        // Registrar los casos de uso
        services.AddScoped<AddToCartUseCase>();
        services.AddScoped<CreateDrinkUseCase>();
        services.AddScoped<CreateOrderFromCartUseCase>();
        services.AddScoped<DeleteDrinkUseCase>();
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<GetAvailableDrinksUseCase>();
        services.AddScoped<GetCartByIdUseCase>();
        services.AddScoped<GetDrinkByIdUseCase>();
        services.AddScoped<GetOrderByIdUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<GetUserOrdersUseCase>();
        services.AddScoped<RemoveFromCartUseCase>();
        services.AddScoped<UpdateDrinkUseCase>();
        services.AddScoped<UpdateOrderStatusUseCase>();

        return services;
    }
}
