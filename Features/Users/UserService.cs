using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ERP.Shared.Commons;
using ERP.Shared.Tenants;
using ERP.Shared.Users;
using ERP.Ticketing.HttpApi.Commons;
using ERP.Ticketing.HttpApi.Commons.Exceptions;
using ERP.Ticketing.HttpApi.Commons.Extensions;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Tenants;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using ERP.Ticketing.HttpApi.Services;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Opw.HttpExceptions;

namespace ERP.Ticketing.HttpApi.Features.Users;

public class UserService
{
	private readonly AppDbContext _dbContext;
	private readonly UserManager<User> _userManager;
	private readonly IOptions<JwtConfig> _jwtConfig;
	private readonly ISmsSenderService _smsSenderService;
	private readonly IHttpContextAccessor _contextAccessor;

	public UserService(AppDbContext dbContext, UserManager<User> userManager, IOptions<JwtConfig> jwtConfig, ISmsSenderService smsSenderService, IHttpContextAccessor contextAccessor)
    {
	    _dbContext = dbContext;
	    _userManager = userManager;
	    _jwtConfig = jwtConfig;
	    _smsSenderService = smsSenderService;
	    _contextAccessor = contextAccessor;
    }

    public async Task<PagedResult<UserListDto>> ListOfUsers(UserListFilter filters, CancellationToken cancellationToken = default)
    {
	    var predicate = PredicateBuilder.New<User>(true);

	    if (filters.Q is { Length: > 0 })
	    {
		    predicate = predicate.And(x => x.Name!.Contains(filters.Q));
	    }
	    
	    if (filters.FullName is { Length: > 0 })
	    {
		    predicate = predicate.And(x => (x.Name + " " + x.Family).Contains(filters.FullName));
	    }
	    
	    if (filters.UserName is { Length: > 0 })
	    {
		    predicate = predicate.And(x => x.UserName!.Contains(filters.UserName));
	    }
	    
	    if (filters.Email is { Length: > 0 })
	    {
		    predicate = predicate.And(x => x.Email!.Contains(filters.Email));
	    }
	    
	    return await _dbContext.Users.AsExpandable()
		    .Where(predicate)
		    .Where(x => x.TenantUsers.Any(t => t.TenantId == _contextAccessor.HttpContext!.GetTenantId()))
		    .Select(UserMapper.ListMapper)
		    .SortAndPagedResultAsync(filters, cancellationToken);
    }
    
    public async Task RequestOtpAsync(string mobile, CancellationToken token = default)
	{
		var user = await _userManager.Users
			// .Where(x => x.Tenants.Any(t => t.Id == _contextAccessor.HttpContext!.GetTenantId()))
			.FirstOrDefaultAsync(x => x.UserName == mobile, token);

		if (user == null)
		{
			user = new User(mobile);
			var result = await _userManager.CreateAsync(user);

			if (!result.Succeeded)
				throw new IdentityException(result);
		}

		var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider);
		await _smsSenderService.SendLookupAsync(mobile, code, "weezh", cancellationToken: token);
	}

	public async Task<TokenResponse> VerifyOtpAsync(string mobile, string code, CancellationToken cancellationToken = default)
	{
		var user = await _userManager.Users
			// .Where(x => x.Tenants.Any(t => t.Id == _contextAccessor.HttpContext!.GetTenantId()))
			.Include(x => x.UserRoles)
			.ThenInclude(x => x.Role)
			.ThenInclude(x => x.Permissions)
			.Include(x => x.TenantUsers)
			.ThenInclude(x => x.Tenant)
			.FirstOrDefaultAsync(x => x.UserName == mobile, cancellationToken);

		if (user == null)
		{
			throw new NotFoundException(mobile + " not found");
		}

		// if (!await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultPhoneProvider, code))
		// 	throw new BadRequestException("invalid data");

		var claims = await GetClaims(user);

		return GetToken(claims, user);
	}

	private TokenResponse GetToken(IEnumerable<Claim> claims, User user)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Value.Key));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

		var token = new JwtSecurityToken(
			issuer: _jwtConfig.Value.Issuer,
			audience: _jwtConfig.Value.Issuer,
			claims: claims,
			expires: DateTime.UtcNow.AddMinutes(_jwtConfig.Value.Expire),
			signingCredentials: credentials
		);
		var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
		
		return new TokenResponse()
		{
			AccessToken = accessToken,
			RefreshToken = GenerateRefreshToken(),
			Expires = token.ValidTo.Ticks,
			User = new UserDto()
			{
				Id = user.Id,
				UserName = user.UserName,
				Family = user.Family,
				Name = user.Name,
				Picture = user.Picture,
				PhoneNumber = user.PhoneNumber
			},
			Roles = user.UserRoles.Select(x => x.Role).Select(x => x.Name!).ToList(),
			Permissions = user.UserRoles.Select(x => x.Role).SelectMany(x => x.Permissions).Select(x => x.Name).ToHashSet(),
			Tenants = user.TenantUsers.Select(x=> new TenantDto
			{
				Id = x.Tenant.Id,
				Title = x.Tenant.Title
			}).ToList()
		};
	}

	private string GenerateRefreshToken()
	{
		var randomNumber = new byte[32];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	private async Task<List<Claim>> GetClaims(User user)
	{
		var claims = new List<Claim>();

		claims.AddRange(new[]
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
		});

		// var roles = await _userManager.GetRolesAsync(user);
		// claims.AddRange(roles.Select(x => new Claim("role", x)));
		// var roles = await user.Roles.AsQueryable()
		// 	.Include(x => x.Permissions)
		// 	.ToListAsync();

		// if (user.Roles.Count > 0)
		// {
		// 	claims.AddRange(user.Roles.Select(x => new Claim("roles", x.Name)));
		//
		// 	var permissions = new List<string>();
		// 	foreach (var role in user.Roles)
		// 	{
		// 		foreach (var permission in role.Permissions)
		// 		{
		// 			if (!permissions.Contains(permission.Name))
		// 			{
		// 				permissions.Add(permission.Name);
		// 			}
		// 		}
		// 	}
		// 	
		// 	if (permissions.Count > 0)
		// 	{
		// 		claims.AddRange(permissions.Select(x => new Claim(CustomClaim.Permissions, x)));
		// 	}
		// }

		return claims;
	}

	public async Task<UserDto> Create(UserCreateDto request, CancellationToken cancellationToken = default)
	{
		var user = await _dbContext.Users
			.Include(x => x.TenantUsers)
			.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);
		
		if (user != null && user.TenantUsers.Any(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId()))
		{
			throw new MobileAlreadyExists();
		}
		
		var userExists = user != null;
		user ??= new User(request.UserName)
		{
			UserName = request.UserName,
			PhoneNumber = request.UserName,
			Email = request.Email,
			Name = request.Name,
			Family = request.Family
		};

		if (request.Roles != null)
		{
			user.UserRoles = await _dbContext.Roles.Where(x => request.Roles.Contains(x.Id)).Select(x => new UserRole { RoleId = x.Id}).ToListAsync(cancellationToken);
		}

		var tenant =
			await _dbContext.Tenants.FirstOrDefaultAsync(x => x.Id == _contextAccessor.HttpContext!.GetTenantId(),
				cancellationToken);

		if (tenant != null)
		{
			user.TenantUsers.Add(new TenantUser { TenantId = _contextAccessor.HttpContext!.GetTenantId()! });
		}

		if (userExists)
		{
			_dbContext.Users.Update(user);
		}
		else
		{
			_dbContext.Users.Add(user);
		}
		
		await _dbContext.SaveChangesAsync(cancellationToken);

		return user.Adapt<UserDto>();
	}

	public async Task<UserDto> Update(Guid userId, UpdateUserDto request, CancellationToken cancellationToken = default)
	{
		if (await _dbContext.Users.Where(x => x.TenantUsers.AsQueryable().Any(t => t.TenantId == _contextAccessor.HttpContext!.GetTenantId())).FirstOrDefaultAsync(x => x.UserName == request.UserName && x.Id != userId, cancellationToken) != null)
		{
			throw new MobileAlreadyExists();
		}

		var user = await _dbContext.Users
			.Where(x => x.TenantUsers.AsQueryable().Any(t => t.TenantId == _contextAccessor.HttpContext!.GetTenantId()))
			.Include(x => x.UserRoles.AsQueryable().Where(x => x.TenantId == _contextAccessor.HttpContext!.GetTenantId()))
			.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

		if (user == null)
		{
			throw new UserNotFoundException();
		}

		user.Name = request.Name;
		user.Family = request.Family;
		user.Email = request.Email;
		user.UserName = request.UserName;
		user.PhoneNumber = request.UserName;
		
		user.UserRoles.Clear();
		if (request.Roles != null)
		{
			user.UserRoles = await _dbContext.Roles.Where(x => request.Roles.Contains(x.Id)).Select(x => new UserRole { RoleId = x.Id}).ToListAsync(cancellationToken);
		}

		_dbContext.Users.Update(user);
		
		await _dbContext.SaveChangesAsync(cancellationToken);

		return user.Adapt<UserDto>();
	}

	public async Task Delete(Guid userId, CancellationToken cancellationToken = default)
	{
		var user = await _dbContext.Users
			.Where(x => x.TenantUsers.AsQueryable().Any(t => t.TenantId == _contextAccessor.HttpContext!.GetTenantId()))
			.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

		if (user == null)
		{
			throw new UserNotFoundException();
		}

		user.DeletedBy = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == _contextAccessor.HttpContext!.User.GetUserId(), cancellationToken);
		user.DeletedAt = DateTime.Now;

		_dbContext.Users.Update(user);
		
		await _dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task<UserDto> GetById(Guid userId, CancellationToken cancellationToken = default)
	{
		var user = await _dbContext.Users
			.Where(x => x.TenantUsers.AsQueryable().Any(t => t.TenantId == _contextAccessor.HttpContext!.GetTenantId()))
			.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

		if (user == null)
		{
			throw new UserNotFoundException();
		}
		
		return user.Adapt<UserDto>();
	}
}