using API.Models;
using API.Services;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            ApiResult<UserDataResult> usersResult = await _userService.GetUsersInfo();
            var userList = new List<User>();

            usersResult.Match(
                whenOk: usersResult =>
                {
                    switch (usersResult)
                    {
                        case UserInfoSuccessResult successResult:
                            userList = successResult.Users.ToList();
                            break;
                        case UserDataFailureResult userDataFailureResult:
                            break;
                    }
                },
                whenException: exceptionMessage =>
                {
                    
                });

            return Ok(userList);
        }
        [HttpGet("top-colours")]
        public async Task<ActionResult<IEnumerable<UserTopColours>>> GetUserTopColours()
        {
            ApiResult<UserDataResult> usersResult = await _userService.GetUsersInfo();
            var userTopColoursList = new List<UserTopColours>();

            usersResult.Match(WhenOk, WhenException);

            async void WhenOk(UserDataResult userDataResult)
            {
                switch (userDataResult)
                {
                    case UserInfoSuccessResult userInfoSuccessResult:
                        userTopColoursList = userInfoSuccessResult.UserTopColours.ToList();
                        break;
                    case UserDataFailureResult userDataFailureResult:
                        
                        break;
                }
            }
            async void WhenException(ApiExceptionResult apiExceptionResult)
            {
                await Console.Out.WriteLineAsync("Failed to get user top colours");
            }
            return userTopColoursList;
        }
        [HttpGet("age-plus-twenty")]
        public async Task<ActionResult<IEnumerable<UserAgePlusTwenty>>> GetUserAgePlusTwenty()
        {
            ApiResult<UserDataResult> usersResult = await _userService.GetUsersInfo();
            var userAgePlusTwentyList = new List<UserAgePlusTwenty>();

            usersResult.Match(WhenOk, WhenException);

            async void WhenOk(UserDataResult userDataResult)
            {
                switch (userDataResult)
                {
                    case UserInfoSuccessResult userInfoSuccessResult:
                        userAgePlusTwentyList = userInfoSuccessResult.UserAgesPlusTwenty.ToList();
                        break;
                    case UserDataFailureResult userDataFailureResult:
                        
                        break;
                }
            }
            async void WhenException(ApiExceptionResult apiExceptionResult)
            {
                await Console.Out.WriteLineAsync("Failed to get users age plus 20");
            }
            return userAgePlusTwentyList;
        }
    }
}