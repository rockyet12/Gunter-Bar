using GunterBar.Application.Common.Models;

namespace GunterBar.Application.UseCases.Common;

public abstract class UseCase<TRequest, TResponse>
{
    protected abstract Task<ApiResponse<TResponse>> ExecuteAsync(TRequest request);

    public async Task<ApiResponse<TResponse>> Execute(TRequest request)
    {
        try
        {
            return await ExecuteAsync(request);
        }
        catch (Exception ex)
        {
            return ApiResponse<TResponse>.Fail($"Error al ejecutar el caso de uso: {ex.Message}");
        }
    }
}

public abstract class UseCase<TResponse>
{
    protected abstract Task<ApiResponse<TResponse>> ExecuteAsync();

    public async Task<ApiResponse<TResponse>> Execute()
    {
        try
        {
            return await ExecuteAsync();
        }
        catch (Exception ex)
        {
            return ApiResponse<TResponse>.Fail($"Error al ejecutar el caso de uso: {ex.Message}");
        }
    }
}
