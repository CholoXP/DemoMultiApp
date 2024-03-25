using DemoMultiApp.Core.Interface;
using DemoMultiApp.Data.Context;
using DemoMultiApp.Data.Model;
using DemoMultiApp.Data.ViewModel.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMultiApp.Core.Reposiotry
{
    public class UserRepository : IUserRepository
    {
        private readonly DemoDbContext _context;
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _configuration;
        public UserRepository(DemoDbContext context, UserManager<UserModel> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            bool result = false;
            UserModel user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                var disable = await _userManager.UpdateAsync(user);
                if (disable.Succeeded)
                    result = true;
            }
            return result;
        }

        public async Task<List<UserGetViewModel>> GetAllActiveUsers()
        {
            List<UserGetViewModel> users = await (from user in _context.Users
                                                  where user.IsActive.Equals(true)
                                                  select new UserGetViewModel
                                                  {
                                                      Id = user.Id,
                                                      FirstName = user.FirstName ?? string.Empty,
                                                      LastName = user.LastName ?? string.Empty,
                                                      Email = user.Email ?? string.Empty
                                                  }).ToListAsync();
            return users;
        }

        public async Task<bool> PostUserAsync(UserModel user, string password, string roleName)
        {
            bool result = false;
            try
            {
                user.IsActive = true;
                var create = await _userManager.CreateAsync(user, password);
                if (create.Succeeded)
                {
                    var addToRole = await _userManager.AddToRoleAsync(user, roleName);
                    if (addToRole.Succeeded)
                        result = true;
                    else
                        await _userManager.DeleteAsync(user);
                }
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public async Task<bool> PutUserAsync(UserModel user, string? roleName)
        {
            bool result = false;
            if (user != null)
            {
                var update = await _userManager.UpdateAsync(user);
                if (update.Succeeded)
                {
                    var role = await (from a in _context.UserRoles
                                      join b in _context.Roles on a.RoleId equals b.Id
                                      where a.UserId.Equals(user.Id)
                                      select b.Name).FirstOrDefaultAsync();
                    var updateRole = await _userManager.RemoveFromRoleAsync(user, role);
                    if (updateRole.Succeeded)
                        result = true;
                }
            }
            return result;
        }
    }
}
