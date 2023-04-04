using ERP.Shared.Tenants;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Tenants;

public class TenantService
{
    private readonly AppDbContext _dbContext;

    public TenantService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Create(CreateTenantDto request, CancellationToken cancellationToken = default)
    {
        _dbContext.Tenants.Add(new Tenant()
        {
            Id = request.Name,
            Title = request.Title
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Update(string tenantId, CreateTenantDto request, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId, cancellationToken);

        if (tenant == null)
        {
            throw new TenantNotFoundException();
        }

        tenant.Id = request.Name;
        tenant.Title = request.Title;
        
        _dbContext.Tenants.Update(tenant);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Delete(string tenantId, CancellationToken cancellationToken = default)
    {
        var tenant = await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == tenantId, cancellationToken);

        if (tenant == null)
        {
            throw new TenantNotFoundException();
        }
        
        _dbContext.Tenants.Remove(tenant);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}