using AuthService.CustomAuthorizationAttributes;
using AuthService.Services;
using AuthService.Services.Abstractions;
using AuthService.UseCases.Commands.DeleteUserById;
using AuthService.UseCases.Commands.EditUser;
using AuthService.Users.Commands.CheckVerificationCode;
using AuthService.Users.Commands.CreateUser;
using AuthService.Users.Commands.SendVerificationCode;
using AuthService.Users.Queries.LogInUser;
using AuthService.Users.Queries.RefreshSession;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IMemoryCache _memoryCache;
        private readonly IMemoryCachingHandler _cachingHandler;

        public AuthController(ISender sender, IMemoryCache memoryCache, IMemoryCachingHandler cachingHandler) 
        {
            _sender = sender;
            _memoryCache = memoryCache;
            _cachingHandler = cachingHandler;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUser([FromBody]CreateUserCommand createUserCommand)
        {
            await _sender.Send(createUserCommand);
            return Ok("User Created");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody]LoginUserQuery loginUserQuery)
        {
            var jwtKey = await _sender.Send(loginUserQuery);
            return Ok(jwtKey);
        }

        [HttpPost]
        [Route("refreshSession")]
        public async Task<IActionResult> RefreshSession()
        {
            var jwtToken = await _sender.Send(new RefreshSessionQuery());
            return Ok(jwtToken);
        }


        [HttpPost]
        [Route("sendVerificationCode")]
        [Authorize(Policy = "RequireUserIsNotVerified")]
        public async Task<IActionResult> SendVerificationCode()
        {
            await _sender.Send(new GenerateVerificationCodeCommand());
            return Ok("Verification Code Sent");
        }

        [HttpPost]
        [Route("checkVerificationCode")]
        [Authorize(Policy = "RequireUserIsNotVerified")]
        public async Task<IActionResult> CheckVerificationCode([FromBody] CheckVerificationCodeCommand checkVerificationCommand)
        {
            await _sender.Send(checkVerificationCommand);
            return Ok("Account verified");
        }

        [HttpDelete]
        [Route("deleteUser")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> DeleteUserByUserId()
        {
            await _sender.Send(new DeleteUserByIdCommand());
            return Ok("Account deleted");
        }

        [HttpPatch]
        [Route("editUser")]
        [Authorize(Policy = "RequireUserIsVerifiedAndWithIdInClaims")]
        public async Task<IActionResult> EditUser(EditUserCommand editUserCommand)
        {
            await _sender.Send(editUserCommand);
            return Ok("User edited");
        }

    }
}
