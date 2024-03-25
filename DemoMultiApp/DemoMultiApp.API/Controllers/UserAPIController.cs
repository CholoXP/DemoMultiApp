using AutoMapper;
using DemoMultiApp.Core.Interface;
using DemoMultiApp.Data.Model;
using DemoMultiApp.Data.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DemoMultiApp.API.Controllers
{
    [ApiController]
    [Route("/API/User")]
    [Authorize]
    public class UserAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly UserManager<UserModel> _userManager;
        private readonly IFunctionRepository _functionRepository;
        private readonly IUserRepository _userRepository;
        public UserAPIController(IMapper mapper, IConfiguration configuration, UserManager<UserModel> userManager, IFunctionRepository functionRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            _functionRepository = functionRepository;
            _userRepository = userRepository;
        }
        [HttpPost("PostUser")]
        public async Task<IActionResult> PostUser([FromBody] UserPostViewModel jsonData)
        {
            var tokenData = HttpContext.User.Identity as ClaimsIdentity;
            bool isValid = await _functionRepository.VerifyToken(tokenData);
            if (!isValid)
                return Unauthorized();
            if (jsonData == null)
                return BadRequest(new { Message = "Json Data is empty" });
            UserModel user = _mapper.Map<UserModel>(jsonData);
            if (user == null || string.IsNullOrWhiteSpace(jsonData.Password) || string.IsNullOrWhiteSpace(jsonData.RoleName))
                return BadRequest(new { Message = "Some fields are required" });
            bool result = await _userRepository.PostUserAsync(user, jsonData.Password, jsonData.RoleName);
            return result ? Ok(new { Message = "Success" }) : BadRequest(new { Message = "Error" });
        }
        [HttpGet("GetActiveUsers")]
        public async Task<IActionResult> GetAllActiveUsers()
        {
            var tokenData = HttpContext.User.Identity as ClaimsIdentity;
            bool isValid = await _functionRepository.VerifyToken(tokenData);
            if (isValid)
                return Ok(await _userRepository.GetAllActiveUsers());
            else
                return Unauthorized();
        }
        [HttpPut("PutUser")]
        public async Task<IActionResult> PutUser([FromBody] UserPutViewModel jsonData)
        {
            var tokenData = HttpContext.User.Identity as ClaimsIdentity;
            bool isValid = await _functionRepository.VerifyToken(tokenData);
            if (!isValid)
                return Unauthorized();
            if (jsonData == null)
                return BadRequest(new { Message = "Json Data is empty" });
            UserModel user = await _userManager.FindByIdAsync(jsonData.Id);
            if (user == null)
                return NotFound();
            _mapper.Map(jsonData, user);
            if (user == null || string.IsNullOrWhiteSpace(jsonData.RoleName))
                return BadRequest(new { Message = "Some fields are required" });
            bool result = await _userRepository.PutUserAsync(user, jsonData.RoleName);
            return result ? Ok(new { Message = "Success" }) : BadRequest(new { Message = "Error" });
        }
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var tokenData = HttpContext.User.Identity as ClaimsIdentity;
            bool isValid = await _functionRepository.VerifyToken(tokenData);
            if (!isValid)
                return Unauthorized();
            if (string.IsNullOrWhiteSpace(Id))
                return BadRequest(new { Message = "Id is empty" });
            bool result = await _userRepository.DeleteUserAsync(Id);
            return result ? Ok(new { Message = "Success" }) : BadRequest(new { Message = "Error" });
        }
    }
}
