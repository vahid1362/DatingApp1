using DatingApp1.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp1
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> optionsBuilder):base(optionsBuilder)
        {

        }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
