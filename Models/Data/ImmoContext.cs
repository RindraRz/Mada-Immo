using System;
using System.Collections.Generic;
using Mada_immo.Models.Csv;
using Microsoft.EntityFrameworkCore;


namespace Mada_immo.Models.Data;
public partial class ImmoContext : DbContext
{

    public ImmoContext()
    {

    }
    public ImmoContext(DbContextOptions<ImmoContext> options) : base(options) { }

    public virtual DbSet<Admin>? Admins { get; set; }

    public virtual DbSet<Bien>? Biens { get; set; }

    public virtual DbSet<BienPhoto>? BienPhotos { get; set; }


    public virtual DbSet<Client>? Clients { get; set; }
    public virtual DbSet<DetailLocation>? DetailLocations { get; set; }
    public virtual DbSet<Location>? Locations { get; set; }

    public virtual DbSet<Proprietaire>? Proprietaires { get; set; }

    public virtual DbSet<TypeBien>? TypeBiens { get; set; }

    public virtual DbSet<BienCsv>? BienCsvs { get; set; }

    public virtual DbSet<LocationCsv>? LocationCsvs { get; set; }

    public virtual DbSet<CommissionCsv>? CommissionCsvs { get; set; }


 public virtual DbSet<Intervall>? Intervalls { get; set; }





    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("name=DefaultConnection");
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.EnableDetailedErrors();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {


        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.AdminId).HasName("admin_pkey");
    
            entity.ToTable("admin");

            entity.Property(e => e.AdminId).HasColumnName("admin_id");

            entity.Property(e => e.Email).HasColumnName("email");

            entity.Property(e => e.Mdp).HasColumnName("mdp");


        });

        modelBuilder.Entity<Bien>(entity =>
       {
           entity.HasKey(e => e.BienId).HasName("bien_pkey");

           entity.ToTable("bien");

           entity.Property(e => e.BienId).HasColumnName("bien_id");

           entity.Property(e => e.Nom).HasColumnName("nom");

           entity.Property(e => e.Reference).HasColumnName("reference");

           entity.Property(e => e.Description).HasColumnName("description");

           entity.Property(e => e.Region).HasColumnName("region");

           entity.Property(e => e.Loyer).HasColumnName("loyer");

           entity.Property(e => e.TypeBienId).HasColumnName("type_bien_id");

           entity.Property(e => e.ProprietaireId).HasColumnName("proprietaire_id");

           entity.HasOne(e => e.ProprietaireIdNavigation).WithMany(p => p.Biens)
                   .HasForeignKey(d => d.ProprietaireId);

           entity.HasOne(e => e.TypeBienIdNavigation).WithMany(p => p.Biens)
                  .HasForeignKey(d => d.TypeBienId);

       });

        modelBuilder.Entity<BienPhoto>(entity =>
      {
          entity.HasKey(e => e.BienPhotoId).HasName("bien_photo_pkey");

          entity.ToTable("bien_photo");

          entity.Property(e => e.BienPhotoId).HasColumnName("photo_id");

          entity.Property(e => e.BienId).HasColumnName("bien_id");

          entity.Property(e => e.Path).HasColumnName("path");


      });

        modelBuilder.Entity<Client>(entity =>
       {
           entity.HasKey(e => e.ClientId).HasName("client_pkey");

           entity.ToTable("client");

           entity.Property(e => e.ClientId).HasColumnName("client_id");

           entity.Property(e => e.Email).HasColumnName("email");


       });

        modelBuilder.Entity<DetailLocation>(entity =>
     {
         entity.HasKey(e => e.DetailLocationId).HasName("detail_location_pkey");

         entity.ToTable("detail_location");

         entity.Property(e => e.DetailLocationId).HasColumnName("detail_location_id");

         entity.Property(e => e.LocationId).HasColumnName("location_id");

         entity.Property(e => e.Loyer).HasColumnName("loyer");

         entity.Property(e => e.Commission).HasColumnName("commission");

         entity.Property(e => e.Mois).HasColumnName("mois");

         entity.Property(e => e.NumMoisLocation).HasColumnName("num_mois_location");
         

         entity.HasOne(e => e.LocationIdNavigation).WithMany(p => p.DetailLocations)
                  .HasForeignKey(d => d.LocationId);


     });

        modelBuilder.Entity<Location>(entity =>
    {
        entity.HasKey(e => e.LocationId).HasName("location_pkey");

        entity.ToTable("location");

        entity.Property(e => e.LocationId).HasColumnName("location_id");

        entity.Property(e => e.ClientId).HasColumnName("client_id");

        entity.Property(e => e.Duree).HasColumnName("duree");

        entity.Property(e => e.DateDebut).HasColumnName("date_debut");
        entity.Property(e => e.DateFin).HasColumnName("date_fin");

        entity.Property(e => e.BienId).HasColumnName("bien_id");

        entity.HasOne(e => e.BienIdNavigation).WithMany(p => p.Locations)
                 .HasForeignKey(d => d.BienId);

    });


        modelBuilder.Entity<Proprietaire>(entity =>
    {
        entity.HasKey(e => e.ProprietaireId).HasName("proprietaire_pkey");

        entity.ToTable("proprietaire");

        entity.Property(e => e.ProprietaireId).HasColumnName("proprietaire_id");

        entity.Property(e => e.Contact).HasColumnName("contact");


    });

        modelBuilder.Entity<TypeBien>(entity =>
    {
        entity.HasKey(e => e.TypeBienId).HasName("type_bien_pkey");

        entity.ToTable("type_bien");

        entity.Property(e => e.TypeBienId).HasColumnName("type_bien_id");

        entity.Property(e => e.Nom).HasColumnName("nom");

        entity.Property(e => e.Commission).HasColumnName("commission");
    });

        modelBuilder.Entity<BienCsv>(entity =>
           {
               entity.HasKey(e => e.BienCsvId).HasName("bien_csv_pkey");
               entity.ToTable("bien_csv");

               entity.Property(e => e.BienCsvId).HasColumnName("bien_csv_id");
               entity.Property(e => e.Reference).HasColumnName("reference");

               entity.Property(e => e.Nom).HasColumnName("nom");

               entity.Property(e => e.Description).HasColumnName("description");

               entity.Property(e => e.Type).HasColumnName("type");
               entity.Property(e => e.Region).HasColumnName("region");
               entity.Property(e => e.Loyer).HasColumnName("loyer");
               entity.Property(e => e.Proprietaire).HasColumnName("proprietaire");


           });

        modelBuilder.Entity<LocationCsv>(entity =>
        {
            entity.HasKey(e => e.LocationCsvId).HasName("location_csv_pkey");
            entity.ToTable("location_csv");

            entity.Property(e => e.LocationCsvId).HasColumnName("location_csv_id");
            entity.Property(e => e.Reference).HasColumnName("reference");

            entity.Property(e => e.DateDebut).HasColumnName("date_debut");

            entity.Property(e => e.Duree).HasColumnName("duree");

            entity.Property(e => e.Client).HasColumnName("client");



        });
        modelBuilder.Entity<CommissionCsv>(entity =>
        {
            entity.HasKey(e => e.CommissionCsvId).HasName("commission_csv_pkey");
            entity.ToTable("commission_csv");

            entity.Property(e => e.CommissionCsvId).HasColumnName("commission_csv_id");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.Property(e => e.Commission).HasColumnName("commission");




        });
        modelBuilder.Entity<Intervall>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("interval_pkey");
            entity.ToTable("interval");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Min).HasColumnName("min");
            entity.Property(e => e.Max).HasColumnName("max");
        });




        OnModelCreatingPartial(modelBuilder);

    }
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}


