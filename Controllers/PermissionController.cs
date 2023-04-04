using System.Text;
using ERP.Shared.Roles;
using Microsoft.AspNetCore.Mvc;
using ERP.Ticketing.HttpApi.Features.Users;
using ERP.Ticketing.HttpApi.Controllers.Common;

namespace ERP.Ticketing.HttpApi.Controllers;

[Route("api/users/roles/permissions")]
public class PermissionController: AuthApiControllerBase
{
    private readonly PermissionService _permissionService;
    private readonly IEnumerable<EndpointDataSource> _endpointDataSources;

    public PermissionController(PermissionService permissionService, IEnumerable<EndpointDataSource> endpointDataSources)
    {
        _permissionService = permissionService;
        _endpointDataSources = endpointDataSources;
    }

    [HttpGet(Name = "all_permissions")]
    public async Task<IActionResult> All()
    {
        var result = await _permissionService.All();
        return Ok(result);
    }

    [HttpPost("list", Name = "permissions_list")]
    public async Task<IActionResult> ListOfPermissions()
    {
        var sb = new StringBuilder();
        var endpoints = _endpointDataSources.SelectMany(es => es.Endpoints);

        var perms = await _permissionService.All();
        var permissions = new List<PermissionDto>();

        foreach (var endpoint in endpoints)
        {
            // if (endpoint is RouteEndpoint routeEndpoint)
            // {
            //     _ = routeEndpoint.RoutePattern.RawText;
            //     _ = routeEndpoint.RoutePattern.PathSegments;
            //     _ = routeEndpoint.RoutePattern.Parameters;
            //     _ = routeEndpoint.RoutePattern.InboundPrecedence;
            //     _ = routeEndpoint.RoutePattern.OutboundPrecedence;
            //
            //     var perm = perms.FirstOrDefault(x => x.Route == routeEndpoint.RoutePattern.RawText!.ToLower());
            //     
            //     Console.WriteLine(routeEndpoint.RoutePattern.RawText!.ToLower());
            //     if (perm == null)
            //     {
            //         perm = new PermissionDto()
            //         {
            //             Route = routeEndpoint.RoutePattern.RawText!.ToLower()
            //         };
            //     }
            //     
            //     permissions.Add(perm);
            // }

            var routeName = endpoint.Metadata.OfType<RouteNameMetadata>().FirstOrDefault()?.RouteName?.ToLower();
            if (routeName != null)
            {
                var perm = perms.FirstOrDefault(x => x.Route == routeName);
                if (perm == null)
                {
                    perm = new PermissionDto()
                    {
                        Route = routeName
                    };
                }
                
                permissions.Add(perm);
            }
            
            //
            // var httpMethodMetaData = endpoint.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault();
            // _ = httpMethodMetaData?.HttpMethods;
        }
        
        return Ok(permissions);
    }

    [HttpPost("sync_perms", Name = "sync_permissions")]
    public async Task<IActionResult> SyncPermissions([FromForm] SyncPermissionRequestDto request)
    {
        await _permissionService.SyncPermissions(request);
        return Accepted();
    }
}