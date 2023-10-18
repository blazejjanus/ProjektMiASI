using DB.DBO;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DB {
    public class DataContext : DbContext {
        private Config Config { get; }
        
        public DataContext() { 
            Config = Config.ReadConfig();
        }

        public DataContext(Config config) {
            Config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetInstance().RootPath);
            optionsBuilder.UseLazyLoadingProxies().UseSqlServer(Config.ConnectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserDBO>().HasOne(x => x.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JwtDBO>().HasOne(x => x.User).WithOne().OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<UserDBO> Users { get; set; }
        public DbSet<AddressDBO> Address { get; set; }
        public DbSet<JwtDBO> Jwt { get; set; }
        public DbSet<EventDBO> Events { get; set; }
    }
}
