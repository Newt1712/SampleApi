using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Domains.Entities;

namespace Web.Infrastructure.DBContext
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration configuration;
       
        public DatabaseContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("DbConnection"), builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<ClubMembership>()
              .HasOne(cm => cm.Club)
              .WithMany(c => c.Memberships)
              .HasForeignKey(cm => cm.ClubId);
        }
        public DbSet<Club> Clubs { get; set; }
        public DbSet<ClubMembership> ClubMemberships { get; set; }
    }
}
