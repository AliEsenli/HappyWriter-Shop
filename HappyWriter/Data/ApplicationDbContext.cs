using System;
using System.Collections.Generic;
using System.Text;
using HappyWriter.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HappyWriter.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HappyWriter.Models.Produkt> Produkte { get; set; }
        public DbSet<HappyWriter.Models.Zubehör> Zubehöre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Produkt>(entity =>
            {

                // Initiale Daten angeben
                entity.HasData(
                    new Produkt { ProduktId = 1, Name = "Etui", Kosten = 15.00m },
                    new Produkt { ProduktId = 2, Name = "Holzschachtel", Kosten = 13.00m }
                    );
            });

            modelBuilder.Entity<Zubehör>(entity =>
            {
                // Initiale Daten angeben
                entity.HasData(
                    new Zubehör { ZubehörId = 1, ZubehörName = "Schere", ZubehörKosten = 7.5m },
                    new Zubehör { ZubehörId = 2, ZubehörName = "Spitzer", ZubehörKosten = 5.5m },
                    new Zubehör { ZubehörId = 3, ZubehörName = "Feder", ZubehörKosten = 10.5m },
                    new Zubehör { ZubehörId = 4, ZubehörName = "Marker", ZubehörKosten = 4.5m },
                    new Zubehör { ZubehörId = 5, ZubehörName = "Lineal", ZubehörKosten = 2.5m },
                    new Zubehör { ZubehörId = 6, ZubehörName = "Papier", ZubehörKosten = 1.5m },
                    new Zubehör { ZubehörId = 7, ZubehörName = "Zirkel", ZubehörKosten = 11.5m }
                    );
            });


            modelBuilder.Entity<KundeProdukt>().HasKey(t => new { t.BestellungId });
            modelBuilder.Entity<KundeZubehör>().HasKey(t => new { t.BestellungId });
            base.OnModelCreating(modelBuilder);
        }

    }
}
