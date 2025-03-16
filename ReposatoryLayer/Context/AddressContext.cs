using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReposatoryLayer.Entity;

namespace ReposatoryLayer.Context
{
    public class AddressContext : DbContext
    {
        public AddressContext(DbContextOptions<AddressContext> options) : base(options) { }

        // DbSet for GreetingMessage entity
        public DbSet<AddressEntity> AddressEntities { get; set; }
        public DbSet<UserEntity> UserEntities { get; set; }

    }
}
