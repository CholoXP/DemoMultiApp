using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DemoMultiApp.Web.APIService.Interface;
using DemoMultiApp.Web.Models.User;

namespace DemoMultiApp.Web.Controllers.API
{
    [AllowAnonymous]
    public class SessionApiController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ISessionService _sessionService;
        public SessionApiController(IMapper mapper, ISessionService sessionService) 
        {
            _mapper = mapper;
            _sessionService = sessionService;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn([FromBody] LoginViewModel jsonData)
        {
            return Ok();
        }
    }
}
