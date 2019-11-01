using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DNDAuth.Models
{
    public class DNDAuthDBContext : DbContext
    {

        //private readonly IConfiguration Configuration;

        //public DNDAuthDBContext(IConfiguration config)
        //{
        //    Configuration = config;
        //}

        public DbSet<User> Users { get; set; }
        public DbSet<SecurityQuestions> SecurityQuestions { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            //string conString = Microsoft.Extensions.Configuration.ConfigurationExtensions.GetConnectionString(this.Configuration, "DefaultConnection");

            optionsBuilder.UseSqlServer("Server=FARHANAMOSTO-PC;Database=DNDAuthDB;Trusted_Connection=True;User Id=sa;Password=123;Integrated Security=false;");
        }
    }
}
