using DemoMultiApp.Core.Interface;
using DemoMultiApp.Data.Context;
using DemoMultiApp.Data.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoMultiApp.Core.Reposiotry
{
    public class FunctionRepository : IFunctionRepository
    {
        private readonly DemoDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        public FunctionRepository(DemoDbContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> VerifyToken(ClaimsIdentity? tokenData)
        {
            bool isValid = false;
            if (tokenData != null)
                isValid = await _userManager.FindByIdAsync(tokenData.Claims.FirstOrDefault(a => a.Type.Equals("Id")).Value ?? string.Empty) != null ? true : false;
            return isValid;
        }
    }
}
