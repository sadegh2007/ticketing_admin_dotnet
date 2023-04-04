using ERP.Shared.Commons;
using ERP.Shared.Departments;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Departments.Mappers;
using LinqKit;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Features.Departments;

public class DepartmentService
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepartmentService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<PagedResult<DepartmentDto>> List(DepartmentListFilter filters, CancellationToken cancellationToken = default)
    {
        var predicate = PredicateBuilder.New<Department>(true);

        // if (filters.IsClosed.HasValue)
        // {
        //     predicate = predicate.And(x => x.IsClosed == filters.IsClosed);
        // }

        if (filters.Title != null)
        {
            predicate = predicate.And(x => x.Title.Contains(filters.Title));
        }
        
        if (filters.Q != null)
        {
            predicate = predicate.And(x => x.Title.Contains(filters.Q));
        }

        return await _dbContext.Departments.AsExpandable()
            .Where(predicate)
            .Where(x => x.TenantId == _httpContextAccessor.HttpContext!.GetTenantId())
            .Select(DepartmentMapper.ListMapper)
            .SortAndPagedResultAsync(filters, cancellationToken);
    }
    
    public async Task<SingleDepartmentDto> GetById(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .Include(x => x.DepartmentUsers)
            .ThenInclude(x => x.User)
            .Where(x => x.TenantId == _httpContextAccessor.HttpContext!.GetTenantId())
            .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

        if (department == null)
        {
            throw new DepartmentNotFoundException();
        }

        return department.Adapt<SingleDepartmentDto>();
    }

    public async Task Create(DepartmentCreateDto request, CancellationToken cancellationToken = default)
    {
        _dbContext.Departments.Add(new Department
        {
            Title = request.Title,
            DepartmentUsers = request.UserIds.Select(x => new DepartmentUser { UserId = x }).ToList()
        });

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Update(Guid departmentId, DepartmentCreateDto request, CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .Include(x => x.DepartmentUsers)
            .Where(x => x.TenantId == _httpContextAccessor.HttpContext!.GetTenantId())
            .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

        if (department == null)
        {
            throw new DepartmentNotFoundException();
        }
        
        department.DepartmentUsers.Clear();
        department.DepartmentUsers = request.UserIds.Select(x => new DepartmentUser { UserId = x }).ToList();
        
        _dbContext.Departments.Update(department);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Delete(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments
            .Where(x => x.TenantId == _httpContextAccessor.HttpContext!.GetTenantId())
            .FirstOrDefaultAsync(x => x.Id == departmentId, cancellationToken);

        if (department == null)
        {
            throw new CategoryNotFoundException();
        }

        var checkDepartmentAssignedInTicket = await _dbContext.Tickets
            .Where(x => x.DepartmentId == departmentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (checkDepartmentAssignedInTicket != null)
        {
            throw new DepartmentAssignedToTicketException();
        }
        
        _dbContext.Departments.Remove(department);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}