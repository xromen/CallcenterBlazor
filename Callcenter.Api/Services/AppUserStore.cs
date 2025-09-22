using Callcenter.Api.Data;
using Callcenter.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Callcenter.Api.Services;

public class AppUserStore(ApplicationDbContext db) : IUserStore<User>, IUserPasswordStore<User>
{
    public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.Id.ToString());

    public Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.Login);

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(userName))
            throw new ArgumentNullException(nameof(userName));
        
        user.Login = userName;
        return Task.CompletedTask;
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(user.Login.ToUpper());

    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
        => Task.CompletedTask;

    public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        db.Users.Add(user);
        return db.SaveChangesAsync(cancellationToken)
            .ContinueWith(_ => IdentityResult.Success, cancellationToken);
    }

    public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        db.Users.Update(user);
        return db.SaveChangesAsync(cancellationToken)
            .ContinueWith(_ => IdentityResult.Success, cancellationToken);
    }

    public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        db.Users.Remove(user);
        return db.SaveChangesAsync(cancellationToken)
            .ContinueWith(_ => IdentityResult.Success, cancellationToken);
    }

    public Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (!int.TryParse(userId, out var id))
        {
            throw new ArgumentException(nameof(userId));
        }
        
        return db.Users
            .Include(c => c.Group)
            .Include(c => c.Organisation)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return db.Users
            .Include(c => c.Group)
            .Include(c => c.Organisation)
            .FirstOrDefaultAsync(c => c.Login.ToUpper() == normalizedUserName, cancellationToken);
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken cancellationToken)
    {
        if(string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentNullException(nameof(passwordHash));
        
        user.Password = passwordHash;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Password);
    }

    public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        => Task.FromResult(!string.IsNullOrEmpty(user.Password));
    
    public void Dispose() { }
}