using Callcenter.Shared;
using Callcenter.Shared.Requests;
using Callcenter.Shared.Responses;
using Callcenter.Web.Models;
using Callcenter.Web.Services.Interfaces;
using UserDto = Callcenter.Shared.UserDto;

namespace Callcenter.Web.Services;

public class AccountsService(IApiClient apiClient)
{
    public async Task<ApiResult> Create(UserCreateDto userDto, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.CreateUser(userDto, cancellationToken);
        
        return await ApiResult.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<List<UserDto>>> Get(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetUsers(cancellationToken);
        
        return await ApiResult<List<UserDto>>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<UserDto>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetUserById(id, cancellationToken);
        
        return await ApiResult<UserDto>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<List<UserGroupDto>>> GetUserGroups(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetUserGroups(cancellationToken);
        
        return await ApiResult<List<UserGroupDto>>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<List<UserNotificationDto>>> GetUserNotifications(CancellationToken cancellationToken = default)
    {
        var response = await apiClient.GetUserNotifications(cancellationToken);
        
        return await ApiResult<List<UserNotificationDto>>.FromResponseAsync(response);
    }
    
    public async Task<ApiResult> ReadUserNotification(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.ReadUserNotification(id, cancellationToken);
        
        return await ApiResult.FromResponseAsync(response);
    }
    
    public async Task<ApiResult> Update(int id, UserCreateDto newsDto, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.UpdateUser(id, newsDto, cancellationToken);
        
        return await ApiResult.FromResponseAsync(response);
    }
    
    public async Task<ApiResult<UserDto>> Delete(int id, CancellationToken cancellationToken = default)
    {
        var response = await apiClient.DeleteUser(id, cancellationToken);
        
        return await ApiResult<UserDto>.FromResponseAsync(response);
    }
}