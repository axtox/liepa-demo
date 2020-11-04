using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;  
using System.Linq;  

namespace LiepaService.Models {
    public class LiepaDemoDatabaseContext : DbContext {
        public LiepaDemoDatabaseContext(DbContextOptions<LiepaDemoDatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Status> UserStatuses { get; set; }
        public DbSet<User> Users { get; set; }
    }
}