using FormulaFlow.Data.Configurations;
using FormulaFlow.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FormulaFlow.Data
{
    public class FormulaFlowContext : IdentityDbContext<ApplicationUser>
    {
        public FormulaFlowContext(DbContextOptions<FormulaFlowContext> options)
            : base(options)
        {
        }

        public DbSet<StockSymbol> StockSymbols { get; set; }
        public DbSet<NetworkCanvas> Canvases { get; set; }
        public DbSet<NetworkCard> Cards { get; set; }
        public DbSet<NetworkCardToNetworkCard> CardsToCards { get; set; }
        public DbSet<NetworkParameter> Parameters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Additional model configuration using Fluent API to mirror data annotations
            builder.ApplyConfiguration(new RoleConfiguration());

            // ApplicationUser explicit configuration to match Identity defaults
            builder.Entity<ApplicationUser>(e =>
            {
                e.HasKey(u => u.Id);

                e.HasIndex(u => u.NormalizedUserName)
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

                e.HasIndex(u => u.NormalizedEmail)
                    .HasDatabaseName("EmailIndex");
            });

            // StockSymbol: unique index on Symbol, and foreign keys to Created/Updated users
            builder.Entity<StockSymbol>(e =>
            {
                // Primary key
                e.HasKey(x => x.Id);

                e.HasIndex(x => x.Symbol).IsUnique();

                e.HasOne(net => net.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(net => net.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // NetworkCanvas: owner relationship (cascade), created/updated user fks (no action)
            builder.Entity<NetworkCanvas>(e =>
            {
                // Primary key
                e.HasKey(x => x.Id);

                e.HasOne(net => net.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(net => net.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(net => net.OwnerUser)
                    .WithMany(net => net.NetworkCanvas)
                    .HasForeignKey(x => x.OwnerUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


            // NetworkCard: relationship to canvas (cascade), created/updated user fks (no action)
            builder.Entity<NetworkCard>(e =>
            {
                // Primary key
                e.HasKey(x => x.Id);

                e.HasOne(nc => nc.NetworkCanvas)
                    .WithMany(c => c.Cards)
                    .HasForeignKey(nc => nc.NetworkCanvasId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(net => net.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(net => net.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // NetworkCardToNetworkCard: unique composite index on (From, To), and parent/child relationships without cascade
            builder.Entity<NetworkCardToNetworkCard>(e =>
            {
                // Primary key
                e.HasKey(x => x.Id);

                e.HasIndex(x => new { x.From, x.To }).IsUnique();

                e.HasOne(n => n.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(n => n.From)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(n => n.Child)
                    .WithMany(c => c.Parents)
                    .HasForeignKey(n => n.To)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(net => net.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(net => net.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });


            // NetworkParameter: relationship to card (cascade), created/updated user fks (no action)
            builder.Entity<NetworkParameter>(e =>
            {
                // Primary key
                e.HasKey(x => x.Id);

                e.HasOne(p => p.NetworkCard)
                    .WithMany(c => c.Parameters)
                    .HasForeignKey(p => p.NetworkCardId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(net => net.CreatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);

                e.HasOne(net => net.UpdatedByUser)
                    .WithMany()
                    .HasForeignKey(x => x.UpdatedByUserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
