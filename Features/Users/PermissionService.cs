using ERP.Shared.Roles;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class PermissionService
{
    private readonly AppDbContext _dbContext;

    public PermissionService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<PermissionDto>> All(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Permissions
            .Select(PermissionMapper.Mapper)
            .ToListAsync(cancellationToken);
    }

    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var roles = await _dbContext.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.UserRoles)
            .ToArrayAsync(cancellationToken);

        return roles
            .SelectMany(x => x)
            .Select(x => x.Role)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }

    public async Task SyncPermissions(SyncPermissionRequestDto request, CancellationToken cancellationToken = default)
    {
        var currentPermissions = await _dbContext.Permissions.ToListAsync(cancellationToken);
        
        foreach (var perm in request.Perms)
        {
            var current = currentPermissions.FirstOrDefault(x => x.Route == perm.Key);

            if (current == null)
            {
                if (perm.Value.Name == null || perm.Value.Title == null)
                {
                    continue;
                }
                
                current = new Permission() { Route = perm.Key, Name = perm.Value.Name, Title = perm.Value.Title };
                _dbContext.Permissions.Add(current);
            }
            else
            {
                if (perm.Value.Name == null || perm.Value.Title == null)
                {
                    _dbContext.Permissions.Remove(current);
                }
                else
                {
                    current.Name = perm.Value.Name;
                    current.Title = perm.Value.Title;

                    _dbContext.Permissions.Update(current);
                }
            }
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}