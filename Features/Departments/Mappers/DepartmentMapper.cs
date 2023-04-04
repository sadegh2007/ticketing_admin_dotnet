using System.Linq.Expressions;
using ERP.Shared.Departments;

namespace ERP.Ticketing.HttpApi.Features.Departments.Mappers;

public static class DepartmentMapper
{
    public static Expression<Func<Department, DepartmentDto>> ListMapper => department => new DepartmentDto()
    {
        Id = department.Id,
        Title = department.Title,
        CreatedAt = department.CreatedAt
    };
    
    public static Expression<Func<Department, DepartmentDto>> Mapper => department => new DepartmentDto()
    {
        Id = department.Id,
        Title = department.Title,
        CreatedAt = department.CreatedAt
    };
}