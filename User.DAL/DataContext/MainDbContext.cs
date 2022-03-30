using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.DAL.Domains;
using Microsoft.EntityFrameworkCore;

namespace User.DAL.DataContext
{
    public class MainDbContext : DbContext
    {
        public MainDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TP_Person> TP_Person { get; set; }
        public DbSet<TP_ConnectedPerson> TP_ConnectedPeople { get; set; }
        public DbSet<TP_City> TP_City { get; set; }


    }
}
