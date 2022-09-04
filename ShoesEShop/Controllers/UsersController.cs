using System.Net;
using Common;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolManagement;
using PoolManagement.Api.Models.User;
using Service;
using Service.IService;


namespace ShoesEShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtService _jwtService;
    private readonly UserManager<Data.User> _userManager;

    public UsersController(DatabaseContext databaseContext, SignInManager<User> signInManager,
        IJwtService jwtService, UserManager<User> userManager)
    {
        _databaseContext = databaseContext;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<ApiResult<Service.Model.Token>> Login(LoginUser login)
    {
        var user = await _databaseContext.Set<User>().Where(user => user.UserName.ToLower() == login.Username)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return new ApiResult<Service.Model.Token>()
            {
                Message = new[] { "نام کاربری یا رمز عبور اشتباه است" },
                StatusCode = ApiResultStatusCode.AccessDenied,
                IsSuccess = false,
            };
        }
        else
        {
            var result = await _signInManager.PasswordSignInAsync(
                login.Username, login.Password, true, true);
            if (result.Succeeded)
            {
                var token = await _jwtService.GenerateAsync(user);
                return new ApiResult<Service.Model.Token>()
                {
                    Message = new[] { "خوش آمدید" },
                    StatusCode = ApiResultStatusCode.Success,
                    Data = token,
                    IsSuccess = true
                };
            }
            else
            {
                return new ApiResult<Service.Model.Token>()
                {
                    Message = new[] { "نام کاربری یا رمز عبور اشتباه است" },
                    IsSuccess = false,
                    Data = null,
                    StatusCode = ApiResultStatusCode.AccessDenied
                };
            }
        }
    }

    [HttpPost("register-user")]
    public async Task<ApiResult> RegisterUser(CreateUser dto, CancellationToken cancellationToken)
    {
        var user = new User()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = dto.Username,
            FullName = dto.Fullname,
        };

        var register = await _userManager.CreateAsync(user);

        var flag = await _userManager.AddPasswordAsync(user, dto.Password);
        if (register.Succeeded)
        {
            return new ApiResult()
            {
                Message = new[] { "ثبت نام شما با موفقیت انجام شد" },
                StatusCode = ApiResultStatusCode.Success,
                IsSuccess = true
            };
        }
        else
        {
            return new ApiResult()
            {
                StatusCode = ApiResultStatusCode.LogicError,
                Message = register.Errors.Select(s => s.Description).ToArray()!
            };
        }
    }

    [Authorize]
    [HttpGet("user-data")]
    public async Task<ApiResult<UserData>> GetUserData()
    {


        var data = await _databaseContext.Set<User>().Where(c => c.Id == User.Identity!.GetUserId())
            .Select(u => new UserData()
            {
                Fullname = u.FullName,
                Id = u.Id,

            }).FirstAsync();

        return new ApiResult<UserData>()
        {
            Data = data,
            StatusCode = ApiResultStatusCode.Success,
            IsSuccess = true,
            Message = new[] { "عملیات با موفقیت انجام شد" }
        };
    }


}

