using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using GunterBar.Application.Common.Behaviors;
using GunterBar.Application.Common.Validators.Cart;
using GunterBar.Application.Common.Validators.Drinks;
using GunterBar.Application.Common.Validators.Orders;
using GunterBar.Application.DTOs.Cart;
using GunterBar.Application.DTOs.Drinks;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Interfaces;
using GunterBar.Application.Services;
using GunterBar.Application.UseCases.Cart;
using GunterBar.Application.UseCases.Drinks;
using GunterBar.Application.UseCases.Orders;
using GunterBar.Application.UseCases.Users;

namespace GunterBar.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registrar servicios base
        services.AddScoped(typeof(IValidationBehavior<,>), typeof(ValidationBehavior<,>));

        // Registrar validadores
        services.AddScoped<IValidator<CreateDrinkDto>, CreateDrinkDtoValidator>();
        services.AddScoped<IValidator<AddToCartDto>, AddToCartDtoValidator>();
        services.AddScoped<IValidator<CreateOrderDto>, CreateOrderDtoValidator>();

        // Registrar servicios de aplicación
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IDrinkService, DrinkService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IReviewService, ReviewService>();

        // Registrar casos de uso del Carrito
        services.AddScoped<AddToCartUseCase>();
        services.AddScoped<GetCartByIdUseCase>();
        services.AddScoped<RemoveFromCartUseCase>();

        // Registrar casos de uso de Bebidas
        services.AddScoped<CreateDrinkUseCase>();
        services.AddScoped<DeleteDrinkUseCase>();
        services.AddScoped<GetAvailableDrinksUseCase>();
        services.AddScoped<GetDrinkByIdUseCase>();
        services.AddScoped<UpdateDrinkUseCase>();

        // Registrar casos de uso de Órdenes
        services.AddScoped<CreateOrderFromCartUseCase>();
        services.AddScoped<GetOrderByIdUseCase>();
        services.AddScoped<GetUserOrdersUseCase>();
        services.AddScoped<UpdateOrderStatusUseCase>();

        // Registrar casos de uso de Usuarios
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();
        services.AddScoped<AuthenticateUserUseCase>();
        services.AddScoped<ChangePasswordUseCase>();
        services.AddScoped<UpdateUserRoleUseCase>();

        return services;
    }
}
