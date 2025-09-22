using Callcenter.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Callcenter.Api.Services;

public class AppRoleStore : IRoleStore<UserGroup>
{
    public void Dispose()
    {
    }

    public Task<IdentityResult> CreateAsync(UserGroup role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> UpdateAsync(UserGroup role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IdentityResult> DeleteAsync(UserGroup role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRoleIdAsync(UserGroup role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetRoleNameAsync(UserGroup role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetRoleNameAsync(UserGroup role, string? roleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedRoleNameAsync(UserGroup role, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedRoleNameAsync(UserGroup role, string? normalizedName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserGroup?> FindByIdAsync(string roleId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<UserGroup?> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}