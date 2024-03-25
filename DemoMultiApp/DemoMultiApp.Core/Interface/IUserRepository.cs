using DemoMultiApp.Data.Model;
using DemoMultiApp.Data.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMultiApp.Core.Interface
{
    public interface IUserRepository
    {
        Task<bool> DeleteUserAsync(string id);
        Task<List<UserGetViewModel>> GetAllActiveUsers();
        Task<bool> PostUserAsync(UserModel user, string password, string roleName);
        Task<bool> PutUserAsync(UserModel user, string? roleName);
    }
}
