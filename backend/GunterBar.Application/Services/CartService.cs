using GunterBar.Application.DTOs;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

// Implementación del servicio de carrito
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IDrinkRepository _drinkRepository;

    public CartService(ICartRepository cartRepository, IDrinkRepository drinkRepository)
    {
        _cartRepository = cartRepository;
        _drinkRepository = drinkRepository;
    }

    public async Task<ApiResponse<CartDto>> GetCartAsync(int userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);

        if (cart == null)
        {
            // Crear carrito si no existe
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            cart = await _cartRepository.CreateAsync(cart);
        }

        var cartDto = new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                DrinkId = ci.DrinkId,
                DrinkName = ci.Drink.Name,
                DrinkImageUrl = ci.Drink.ImageUrl,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Subtotal = ci.Subtotal,
                AddedAt = ci.AddedAt
            }).ToList(),
            Total = cart.Total,
            TotalItems = cart.TotalItems,
            CreatedAt = cart.CreatedAt
        };

        return new ApiResponse<CartDto>
        {
            Success = true,
            Message = "Carrito obtenido exitosamente",
            Data = cartDto
        };
    }

    public async Task<ApiResponse<CartDto>> AddToCartAsync(int userId, AddToCartDto addToCartDto)
    {
        var drink = await _drinkRepository.GetByIdAsync(addToCartDto.DrinkId);
        if (drink == null)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = "Bebida no encontrada",
                Errors = { "Drink not found" }
            };
        }

        if (!drink.IsAvailable || drink.Stock < addToCartDto.Quantity)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = "Bebida no disponible o stock insuficiente",
                Errors = { "Drink not available or insufficient stock" }
            };
        }

        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            cart = await _cartRepository.CreateAsync(cart);
        }

        var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, addToCartDto.DrinkId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += addToCartDto.Quantity;
            await _cartRepository.UpdateItemAsync(existingItem);
        }
        else
        {
            var newItem = new CartItem
            {
                CartId = cart.Id,
                DrinkId = addToCartDto.DrinkId,
                Quantity = addToCartDto.Quantity,
                UnitPrice = drink.Price,
                AddedAt = DateTime.UtcNow
            };
            await _cartRepository.AddItemAsync(newItem);
        }

        return await GetCartAsync(userId);
    }

    public async Task<ApiResponse<CartDto>> UpdateCartItemAsync(int userId, int cartItemId, UpdateCartItemDto updateDto)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = "Carrito no encontrado",
                Errors = { "Cart not found" }
            };
        }

        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = "Item no encontrado en el carrito",
                Errors = { "Item not found in cart" }
            };
        }

        cartItem.Quantity = updateDto.Quantity;
        await _cartRepository.UpdateItemAsync(cartItem);

        return await GetCartAsync(userId);
    }

    public async Task<ApiResponse<bool>> RemoveFromCartAsync(int userId, int cartItemId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Carrito no encontrado",
                Errors = { "Cart not found" }
            };
        }

        var cartItem = cart.Items.FirstOrDefault(ci => ci.Id == cartItemId);
        if (cartItem == null)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Item no encontrado en el carrito",
                Errors = { "Item not found in cart" }
            };
        }

        await _cartRepository.DeleteItemAsync(cartItemId);

        return new ApiResponse<bool>
        {
            Success = true,
            Message = "Item eliminado del carrito",
            Data = true
        };
    }

    public async Task<ApiResponse<bool>> ClearCartAsync(int userId)
    {
        var cart = await _cartRepository.GetByUserIdAsync(userId);
        if (cart == null)
        {
            return new ApiResponse<bool>
            {
                Success = false,
                Message = "Carrito no encontrado",
                Errors = { "Cart not found" }
            };
        }

        await _cartRepository.ClearCartAsync(cart.Id);

        return new ApiResponse<bool>
        {
            Success = true,
            Message = "Carrito vaciado exitosamente",
            Data = true
        };
    }
}
