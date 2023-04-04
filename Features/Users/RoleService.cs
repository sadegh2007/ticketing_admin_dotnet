using ERP.Shared.Commons;
using ERP.Shared.Roles;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class RoleService
{
    private readonly AppDbContext _dbContext;
    private readonly RoleManager<Role> _roleManager;

    public RoleService(AppDbContext dbContext, RoleManager<Role> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }
    
    public async Task<PagedResult<RoleDto>> ListOfRoles(RoleListFilter filters, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.New<Role>(true);
	    

        if (filters.Q is { Length: > 0 })
        {
            predicate = predicate.And(x => x.Name!.Contains(filters.Q) || x.Title.Contains(filters.Q));
        }
	    
        if (filters.Name is { Length: > 0 })
        {
            predicate = predicate.And(x => x.Name!.Contains(filters.Name));
        }
        
        if (filters.Title is { Length: > 0 })
        {
            predicate = predicate.And(x => x.Title.Contains(filters.Title));
        }
	    
        return await _dbContext.Roles.AsExpandable()
            .Where(predicate)
            .Select(RoleMapper.ListMapper)
            .SortAndPagedResultAsync(filters, cancellationToken);
    }

    public async Task<RoleDto> GetById(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);

        if (role == null)
        {
            throw new RoleNotFoundException();
        }

        return role.Adapt<RoleDto>();
    }

    public async Task<RoleDto> Create(CreateRoleDto request, CancellationToken cancellationToken = default)
    {
        var role = new Role()
        {
            Name = request.Name,
            Title = request.Title,
            Permissions = await _dbContext.Permissions.Where(x => request.Permissions.Contains(x.Id)).ToListAsync(cancellationToken)
        };
        
        _dbContext.Roles.Add(role);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return role.Adapt<RoleDto>();
    }
    
    public async Task<RoleDto> Update(Guid roleId, CreateRoleDto request, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);

        if (role == null)
        {
            throw new RoleNotFoundException();
        }

        role.Name = request.Name;
        role.Title = request.Title;

        // foreach (var permission in role.Permissions)
        // {
        //     role.Permissions.Remove(permission);
        // }
        role.Permissions.Clear();
        role.Permissions = await _dbContext.Permissions.Where(x => request.Permissions.Contains(x.Id)).ToListAsync(cancellationToken);

        _dbContext.Roles.Update(role);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return role.Adapt<RoleDto>();
    }

    public async Task Delete(Guid roleId, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == roleId, cancellationToken);

        if (role == null)
        {
            throw new RoleNotFoundException();
        }

        if (role.Users.AsQueryable().Any())
        {
            throw new Exception("این نقش به کاربر نسبت داده شده و قابل حذف نمی باشد.");
        }

        _dbContext.Roles.Remove(role);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}