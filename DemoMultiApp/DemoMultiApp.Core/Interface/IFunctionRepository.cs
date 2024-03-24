using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoMultiApp.Core.Interface
{
    public interface IFunctionRepository
    {
        Task<bool> VerifyToken(ClaimsIdentity? tokenData);
    }
}
