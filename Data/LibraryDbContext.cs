using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Reader> Readers { get; set; }
        public DbSet<ReaderProfile> ReaderProfiles { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Book> Books { get; set; }


        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Reader>()
                .HasOne(r => r.ReaderProfile)
                .WithOne(rp => rp.Reader)
                .HasForeignKey<ReaderProfile>(rp => rp.ReaderId);

            modelBuilder
                .Entity<Card>()
                .HasOne(c => c.Reader)
                .WithMany(r => r.Cards)
                .HasForeignKey(c => c.ReaderId);

            modelBuilder
                .Entity<History>()
                .HasOne(h => h.Card)
                .WithMany(c => c.Books)
                .HasForeignKey(h => h.CardId);

            modelBuilder
                .Entity<History>()
                .HasOne(h => h.Book)
                .WithMany(c => c.Cards)
                .HasForeignKey(h => h.BookId);
        }
    }
}