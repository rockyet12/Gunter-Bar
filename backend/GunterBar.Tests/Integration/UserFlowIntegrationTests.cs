using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using GunterBar.Presentation;
using GunterBar.Application.DTOs.User;
using GunterBar.Application.DTOs.Order;
using GunterBar.Application.Common.Models;
using System.Collections.Generic;

namespace GunterBar.Tests.Integration;

public class UserFlowIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public UserFlowIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task FullUserFlow_ShouldSucceed()
    {
        using var client = _factory.CreateClient();

        // Paso 1: Registro
        var registerDto = new RegisterDto
        {
            Name = $"Test User {System.Guid.NewGuid()} ",
            Email = $"testuser_{System.Guid.NewGuid()}@gunterbar.com",
            Password = "Test1234!"
        };
        var registerResp = await client.PostAsJsonAsync("/api/auth/register", registerDto);
        registerResp.EnsureSuccessStatusCode();
        var registerData = await registerResp.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
        Assert.True(registerData?.Success);
        Assert.NotNull(registerData?.Data?.Token);


        // Paso 2: Login
        var loginDto = new LoginDto
        {
            Email = registerDto.Email,
            Password = registerDto.Password
        };
        var loginResp = await client.PostAsJsonAsync("/api/auth/login", loginDto);
        loginResp.EnsureSuccessStatusCode();
        var loginData = await loginResp.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
        Assert.True(loginData?.Success);
        var token = loginData?.Data?.Token;
        Assert.False(string.IsNullOrEmpty(token));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Paso extra: Obtener (o crear) el carrito antes de agregar ítems
        var getCartResp = await client.GetAsync("/api/cart/me");
        getCartResp.EnsureSuccessStatusCode();
        // No importa el contenido, solo forzamos la creación

        // Paso 3: Obtener bebidas disponibles
        var drinksResp = await client.GetAsync("/api/drinks");
        drinksResp.EnsureSuccessStatusCode();
        var drinksData = await drinksResp.Content.ReadFromJsonAsync<ApiResponse<List<GunterBar.Application.DTOs.Drinks.DrinkDto>>>();
        Assert.True(drinksData?.Success);
        Assert.NotNull(drinksData?.Data);
        var drink = drinksData.Data!.FirstOrDefault(d => d.IsAvailable && d.Stock > 0);
        Assert.NotNull(drink);

        // Paso 4: Agregar bebida al carrito
        var addToCartDto = new GunterBar.Application.DTOs.Cart.AddToCartDto
        {
            DrinkId = drink.Id,
            Quantity = 1
        };
        var addToCartResp = await client.PostAsJsonAsync("/api/cart/items", addToCartDto);
    addToCartResp.EnsureSuccessStatusCode();
    var cartApiResponse = await addToCartResp.Content.ReadFromJsonAsync<ApiResponse<GunterBar.Application.DTOs.Cart.CartDto>>();
        if (cartApiResponse == null || !cartApiResponse.Success || cartApiResponse.Data == null)
        {
            var mensaje = cartApiResponse?.Message ?? "Sin mensaje";
            var errores = cartApiResponse?.Errors != null ? string.Join(", ", cartApiResponse.Errors) : "Sin errores";
            throw new Exception($"Error al agregar al carrito. Mensaje: {mensaje}. Errores: {errores}");
        }
        Assert.Contains(cartApiResponse.Data.Items, i => i.DrinkId == drink.Id);

        // Paso 5: Crear pedido desde el carrito
        var createOrderDto = new CreateOrderDto
        {
            UserId = loginData?.Data?.User?.Id ?? 0,
            Notes = "Pedido de prueba automatizado"
        };
        var orderResp = await client.PostAsJsonAsync("/api/orders", createOrderDto);
        orderResp.EnsureSuccessStatusCode();
        var orderData = await orderResp.Content.ReadFromJsonAsync<ApiResponse<OrderDto>>();
        Assert.True(orderData?.Success);
        Assert.NotNull(orderData?.Data);

        // Paso 6: Obtener historial de pedidos
        var myOrdersResp = await client.GetAsync("/api/orders/me");
        myOrdersResp.EnsureSuccessStatusCode();
        var myOrdersData = await myOrdersResp.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<OrderDto>>>();
        Assert.True(myOrdersData?.Success);
        Assert.NotNull(myOrdersData?.Data);
        Assert.Contains(myOrdersData.Data, o => o.Id == orderData.Data.Id);
    }
}
