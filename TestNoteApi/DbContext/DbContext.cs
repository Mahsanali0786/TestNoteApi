namespace SampleProject.DbContext
{
    using Microsoft.EntityFrameworkCore;
    using SampleProject.Models;

    public class SampleDbContext : DbContext
    {
    
        public SampleDbContext(DbContextOptions<SampleDbContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
    }

}
