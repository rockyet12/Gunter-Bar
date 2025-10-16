using GunterBar.Application.Common.Models;
using GunterBar.Application.DTOs.Cart;
using GunterBar.Application.Interfaces;
using GunterBar.Domain.Entities;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Application.Services;

// Implementación del servicio de carrito
public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IDrinkRepository _drinkRepository;
    private const int MAX_ITEMS_PER_CART = 20;
    private const int CART_EXPIRATION_HOURS = 24;
    private const int MIN_QUANTITY = 1;
    private const int MAX_QUANTITY = 10;

    public CartService(ICartRepository cartRepository, IDrinkRepository drinkRepository)
    {
        _cartRepository = cartRepository;
        _drinkRepository = drinkRepository;
    }

    private static bool IsCartExpired(Cart cart)
    {
        return (DateTime.UtcNow - cart.UpdatedAt).TotalHours > CART_EXPIRATION_HOURS;
    }

    private static CartDto MapToCartDto(Cart cart)
    {
        if (cart == null) 
            throw new ArgumentNullException(nameof(cart));

        return new CartDto
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(ci => new CartItemDto
            {
                Id = ci.Id,
                DrinkId = ci.DrinkId,
                DrinkName = ci.Drink?.Name ?? string.Empty,
                DrinkImageUrl = ci.Drink?.ImageUrl ?? string.Empty,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Subtotal = ci.Subtotal,
                AddedAt = ci.AddedAt
            }).ToList(),
            Total = cart.Total,
            TotalItems = cart.TotalItems,
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt
        };
    }

    public async Task<ApiResponse<CartDto>> GetCartAsync(int userId)
    {
        try
        {
            var cart = await _cartRepository.GetByUserIdAsync(userId);

            if (cart == null)
            {
                // Crear carrito si no existe
                cart = new Cart(userId);
                cart = await _cartRepository.CreateAsync(cart);
            }
            else if (IsCartExpired(cart))
            {
                // Limpiar carrito si está expirado
                await _cartRepository.ClearCartAsync(cart.Id);
            }

            var cartDto = MapToCartDto(cart);

            return new ApiResponse<CartDto>
            {
                Success = true,
                Message = "Carrito obtenido exitosamente",
                Data = cartDto
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = "Error al obtener el carrito",
                Errors = { ex.Message }
            };
        }
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

        if (addToCartDto.Quantity < MIN_QUANTITY || addToCartDto.Quantity > MAX_QUANTITY)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = $"La cantidad debe estar entre {MIN_QUANTITY} y {MAX_QUANTITY}",
                Errors = { "Invalid quantity" }
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
            cart = new Cart(userId);
            cart = await _cartRepository.CreateAsync(cart);
        }
        else if (IsCartExpired(cart))
        {
            await _cartRepository.ClearCartAsync(cart.Id);
        }

        if (cart.Items.Count >= MAX_ITEMS_PER_CART && 
            !cart.Items.Any(i => i.DrinkId == addToCartDto.DrinkId))
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = $"El carrito ha alcanzado el límite de {MAX_ITEMS_PER_CART} items diferentes",
                Errors = { "Cart is full" }
            };
        }

        var existingItem = await _cartRepository.GetCartItemAsync(cart.Id, addToCartDto.DrinkId);
        
        if (existingItem != null)
        {
            existingItem.Quantity += addToCartDto.Quantity;
            await _cartRepository.UpdateItemAsync(existingItem);
        }
        else
        {
            var newItem = new CartItem(
                cart.Id,
                addToCartDto.DrinkId,
                addToCartDto.Quantity,
                drink.Price
            );
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

        if (updateDto.Quantity < MIN_QUANTITY || updateDto.Quantity > MAX_QUANTITY)
        {
            return new ApiResponse<CartDto>
            {
                Success = false,
                Message = $"La cantidad debe estar entre {MIN_QUANTITY} y {MAX_QUANTITY}",
                Errors = { "Invalid quantity" }
            };
        }

        cartItem.UpdateQuantity(updateDto.Quantity);
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
