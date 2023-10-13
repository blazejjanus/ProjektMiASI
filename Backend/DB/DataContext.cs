using DB.DBO;
using Microsoft.EntityFrameworkCore;

namespace DB {
    public class DataContext : DbContext {
        
        //TODO: OnConfiguration overload, connStr initialization

        public DbSet<UserDBO> Users { get; set; }
    }
}
