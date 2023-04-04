using ERP.Shared.Users;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Data;
using ERP.Ticketing.HttpApi.Features.Users;
using ERP.Ticketing.HttpApi.Features.Users.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.Ticketing.HttpApi.Controllers;

public class AccountController: AuthApiControllerBase
{
    private readonly UserService _userService;
    private readonly AppDbContext _dbContext;

    public AccountController(UserService userService, AppDbContext dbContext)
    {
        _userService = userService;
        _dbContext = dbContext;
    }

    [HttpPost("otp-request"), AllowAnonymous]
    public async Task<IActionResult> RequestOtp([FromBody] OtpRequest request)
    {
        await _userService.RequestOtpAsync(request.Mobile);
        return Ok(new { Message = "کد با موفقیت ارسال شد" });
    }
    
    [HttpPost("otp-verify"), AllowAnonymous]
    public async Task<ActionResult<TokenResponse>> VerifyOtp([FromBody] OtpVerify request)
    {
        return await _userService.VerifyOtpAsync(request.Mobile, request.Code);
    }
    
    [HttpGet(Name = "get_profile")]
    public async Task<UserDto?> GetInfo()
    {
        Console.WriteLine(UserId);
        return await _dbContext.Users.Select(UserMapper.Mapper).FirstOrDefaultAsync(x => x.Id == UserId);
    }
}