using Microsoft.EntityFrameworkCore;
using surer_backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace surer_backend.Database
{
    public class SurerContext:DbContext
    {
        public SurerContext(DbContextOptions<SurerContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
    }
}
