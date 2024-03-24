using DemoMultiApp.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMultiApp.Data.Context
{
    public class DemoDbContext : IdentityDbContext<UserModel, RoleModel, string>
    {
        public DemoDbContext(DbContextOptions<DemoDbContext> options) : base(options) { }
        //Identity Model
        public DbSet<UserModel> Users { get; set; }
        public DbSet<RoleModel> Roles { get; set; }
    }
}
