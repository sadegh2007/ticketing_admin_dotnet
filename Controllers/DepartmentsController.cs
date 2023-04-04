using ERP.Shared.Commons;
using ERP.Shared.Departments;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Departments;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Controllers;

public class DepartmentsController: AuthApiControllerBase
{
    private readonly DepartmentService _departmentService;

    public DepartmentsController(DepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpPost("list", Name = "departments_list")]
    public async Task<PagedResult<DepartmentDto>> List(DepartmentListFilter filters)
    {
        return await _departmentService.List(filters);
    }
    
    [HttpGet("{departmentId:guid}", Name = "get_department_by_id")]
    public async Task<SingleDepartmentDto> GetById(Guid departmentId)
    {
        return await _departmentService.GetById(departmentId);
    }
    
    [HttpPost(Name = "create_department")]
    public async Task<IActionResult> Create(DepartmentCreateDto request)
    {
        await _departmentService.Create(request);
        return Ok();
    }
    
    [HttpPatch("{departmentId:guid}", Name = "update_department")]
    public async Task<IActionResult> Update(Guid departmentId, DepartmentCreateDto request)
    {
        await _departmentService.Update(departmentId, request);
        return Ok();
    }
    
    [HttpDelete("{departmentId:guid}", Name = "delete_department")]
    public async Task<IActionResult> Delete(Guid departmentId)
    {
        await _departmentService.Delete(departmentId);
        return Ok();
    }
}