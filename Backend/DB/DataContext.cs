using DB.DBO;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DB {
    public class DataContext : DbContext {
        private Config? Config { get; }

        public DataContext() : base() { }

        public DataContext(Config config) : base() {
            Config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if (!optionsBuilder.IsConfigured) {
                Config config;
                if(Config != null) {
                    config = Config;
                } else {
                    config = Config.ReadConfig();
                }
                //AppDomain.CurrentDomain.SetData("DataDirectory", Environment.GetInstance().RootPath);
                optionsBuilder.UseLazyLoadingProxies().UseSqlServer(config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserDBO>().HasOne(x => x.Address).WithOne().OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<JwtDBO>().HasOne(x => x.User).WithOne().OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserDBO> Users { get; set; }
        public DbSet<AddressDBO> Address { get; set; }
        public DbSet<JwtDBO> Jwt { get; set; }
        public DbSet<EventDBO> Events { get; set; }
    }
}
