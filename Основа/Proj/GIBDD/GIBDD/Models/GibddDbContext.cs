using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GIBDD.Models;

public partial class GibddDbContext : DbContext
{
    public GibddDbContext()
    {
    }

    public GibddDbContext(DbContextOptions<GibddDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Accident> Accidents { get; set; }

    public virtual DbSet<AccidentParticipant> AccidentParticipants { get; set; }

    public virtual DbSet<Adm> Adms { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Color> Colors { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<EngineType> EngineTypes { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<OtherObject> OtherObjects { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<TypeOfDrive> TypeOfDrives { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=USER;Database=GIBDD_DB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Accident>(entity =>
        {
            entity.Property(e => e.AccidentId)
                .ValueGeneratedNever()
                .HasColumnName("AccidentID");
            entity.Property(e => e.AccidentAddress)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.AccidentClassification)
                .HasMaxLength(228)
                .IsFixedLength();
            entity.Property(e => e.AccidentDate).HasColumnType("datetime");
            entity.Property(e => e.AccidentTime).HasColumnType("datetime");
            entity.Property(e => e.Comments)
                .HasMaxLength(500)
                .IsFixedLength();
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(228)
                .IsFixedLength();
            entity.Property(e => e.ObjectsList)
                .HasMaxLength(228)
                .IsFixedLength();
            entity.Property(e => e.VehiclesList)
                .HasMaxLength(228)
                .IsFixedLength();
        });

        modelBuilder.Entity<AccidentParticipant>(entity =>
        {
            entity.HasKey(e => e.PartId);

            entity.Property(e => e.PartId)
                .ValueGeneratedNever()
                .HasColumnName("PartID");
            entity.Property(e => e.AccidentIdFk).HasColumnName("AccidentID_FK");
            entity.Property(e => e.GuidFk).HasColumnName("GUID_FK");
            entity.Property(e => e.Role)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.AccidentIdFkNavigation).WithMany(p => p.AccidentParticipants)
                .HasForeignKey(d => d.AccidentIdFk)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccidentParticipants_Accidents");

            entity.HasOne(d => d.GuidFkNavigation).WithMany(p => p.AccidentParticipants)
                .HasForeignKey(d => d.GuidFk)
                .HasConstraintName("FK_AccidentParticipants_Drivers");
        });

        modelBuilder.Entity<Adm>(entity =>
        {
            entity.HasKey(e => e.Login);

            entity.ToTable("adm");

            entity.Property(e => e.Login).HasMaxLength(10);
            entity.Property(e => e.Password).HasMaxLength(10);
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarVim);

            entity.Property(e => e.CarVim)
                .HasMaxLength(50)
                .HasColumnName("CarVIM");
            entity.Property(e => e.CarModel).HasMaxLength(45);
            entity.Property(e => e.CarWeight).HasMaxLength(4);
            entity.Property(e => e.CarYear).HasMaxLength(4);

            entity.HasMany(d => d.CarVimFks).WithMany(p => p.EngineIdFks)
                .UsingEntity<Dictionary<string, object>>(
                    "CarEngine",
                    r => r.HasOne<EngineType>().WithMany()
                        .HasForeignKey("CarVimFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_Engine_EngineTypes"),
                    l => l.HasOne<Car>().WithMany()
                        .HasForeignKey("EngineIdFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_Engine_Cars"),
                    j =>
                    {
                        j.HasKey("EngineIdFk", "CarVimFk");
                        j.ToTable("Car_Engine");
                        j.IndexerProperty<string>("EngineIdFk")
                            .HasMaxLength(50)
                            .HasColumnName("EngineID_FK");
                        j.IndexerProperty<string>("CarVimFk")
                            .HasMaxLength(10)
                            .HasColumnName("CarVIM_FK");
                    });
        });

        modelBuilder.Entity<Color>(entity =>
        {
            entity.HasKey(e => e.ColorNum);

            entity.ToTable("Color");

            entity.Property(e => e.ColorNum).HasMaxLength(50);
            entity.Property(e => e.ColorCode).HasMaxLength(7);
            entity.Property(e => e.ColorName).HasMaxLength(50);
            entity.Property(e => e.ColorName1).HasMaxLength(50);

            entity.HasMany(d => d.CarVimFks).WithMany(p => p.ColorNumFks)
                .UsingEntity<Dictionary<string, object>>(
                    "CarColor",
                    r => r.HasOne<Car>().WithMany()
                        .HasForeignKey("CarVimFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_colors_Cars1"),
                    l => l.HasOne<Color>().WithMany()
                        .HasForeignKey("ColorNumFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_colors_Color1"),
                    j =>
                    {
                        j.HasKey("ColorNumFk", "CarVimFk");
                        j.ToTable("Car_colors");
                        j.IndexerProperty<string>("ColorNumFk")
                            .HasMaxLength(50)
                            .HasColumnName("ColorNum_FK");
                        j.IndexerProperty<string>("CarVimFk")
                            .HasMaxLength(50)
                            .HasColumnName("CarVIM_FK");
                    });
        });

        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.Guid);

            entity.Property(e => e.Guid)
                .ValueGeneratedNever()
                .HasColumnName("GUID");
            entity.Property(e => e.ActualAdress).HasMaxLength(30);
            entity.Property(e => e.Address).HasMaxLength(30);
            entity.Property(e => e.Categories).HasMaxLength(10);
            entity.Property(e => e.Company).HasMaxLength(30);
            entity.Property(e => e.Description).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(30);
            entity.Property(e => e.ExpireDate).HasMaxLength(50);
            entity.Property(e => e.JobName).HasMaxLength(30);
            entity.Property(e => e.LicenceDate).HasMaxLength(50);
            entity.Property(e => e.LicenceNumber).HasMaxLength(50);
            entity.Property(e => e.LicenceSeries).HasMaxLength(50);
            entity.Property(e => e.LicenceStatus).HasMaxLength(20);
            entity.Property(e => e.MiddleName).HasMaxLength(15);
            entity.Property(e => e.Name).HasMaxLength(15);
            entity.Property(e => e.PassportNumber).HasMaxLength(10);
            entity.Property(e => e.PassportSeries).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Photo).HasMaxLength(50);
            entity.Property(e => e.Postcode).HasMaxLength(10);
            entity.Property(e => e.Surname).HasMaxLength(15);

            entity.HasMany(d => d.CarVimFks).WithMany(p => p.GuidFks)
                .UsingEntity<Dictionary<string, object>>(
                    "DriversCar",
                    r => r.HasOne<Car>().WithMany()
                        .HasForeignKey("CarVimFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Drivers_Cars_Cars"),
                    l => l.HasOne<Driver>().WithMany()
                        .HasForeignKey("GuidFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Drivers_Cars_Drivers"),
                    j =>
                    {
                        j.HasKey("GuidFk", "CarVimFk");
                        j.ToTable("Drivers_Cars");
                        j.IndexerProperty<long>("GuidFk").HasColumnName("GUID_FK");
                        j.IndexerProperty<string>("CarVimFk")
                            .HasMaxLength(50)
                            .HasColumnName("CarVIM_FK");
                    });
        });

        modelBuilder.Entity<EngineType>(entity =>
        {
            entity.HasKey(e => e.EngineId);

            entity.Property(e => e.EngineId)
                .HasMaxLength(10)
                .HasColumnName("EngineID");
            entity.Property(e => e.EngineName).HasMaxLength(10);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("Manufacturer");

            entity.Property(e => e.ManufacturerId)
                .ValueGeneratedNever()
                .HasColumnName("ManufacturerID");
            entity.Property(e => e.ManufacturerName).HasMaxLength(10);

            entity.HasMany(d => d.CarVimFks).WithMany(p => p.ManufacturerIdFks)
                .UsingEntity<Dictionary<string, object>>(
                    "CarManufacturer",
                    r => r.HasOne<Car>().WithMany()
                        .HasForeignKey("CarVimFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_Manufacturer_Cars"),
                    l => l.HasOne<Manufacturer>().WithMany()
                        .HasForeignKey("ManufacturerIdFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_Manufacturer_Manufacturer"),
                    j =>
                    {
                        j.HasKey("ManufacturerIdFk", "CarVimFk");
                        j.ToTable("Car_Manufacturer");
                        j.IndexerProperty<long>("ManufacturerIdFk").HasColumnName("ManufacturerID_FK");
                        j.IndexerProperty<string>("CarVimFk")
                            .HasMaxLength(50)
                            .HasColumnName("CarVIM_FK");
                    });
        });

        modelBuilder.Entity<OtherObject>(entity =>
        {
            entity.HasKey(e => e.ObjId);

            entity.Property(e => e.ObjId)
                .ValueGeneratedNever()
                .HasColumnName("ObjID");
            entity.Property(e => e.AccidentIdFk).HasColumnName("AccidentID_FK");
            entity.Property(e => e.Comments)
                .HasMaxLength(228)
                .IsFixedLength();
            entity.Property(e => e.ObjectType)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.HasOne(d => d.AccidentIdFkNavigation).WithMany(p => p.OtherObjects)
                .HasForeignKey(d => d.AccidentIdFk)
                .HasConstraintName("FK_OtherObjects_Accidents");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.ToTable("Region");

            entity.Property(e => e.RegionId)
                .ValueGeneratedNever()
                .HasColumnName("RegionID");
            entity.Property(e => e.RegionCode).HasMaxLength(30);
            entity.Property(e => e.RegionName).HasMaxLength(40);

            entity.HasMany(d => d.CarVimFks).WithMany(p => p.RegionIdFks)
                .UsingEntity<Dictionary<string, object>>(
                    "CarRegion",
                    r => r.HasOne<Car>().WithMany()
                        .HasForeignKey("CarVimFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_Region_Cars"),
                    l => l.HasOne<Region>().WithMany()
                        .HasForeignKey("RegionIdFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_Region_Region"),
                    j =>
                    {
                        j.HasKey("RegionIdFk", "CarVimFk");
                        j.ToTable("Car_Region");
                        j.IndexerProperty<long>("RegionIdFk").HasColumnName("RegionID_FK");
                        j.IndexerProperty<string>("CarVimFk")
                            .HasMaxLength(50)
                            .HasColumnName("CarVIM_FK");
                    });
        });

        modelBuilder.Entity<TypeOfDrive>(entity =>
        {
            entity.HasKey(e => e.Todid);

            entity.ToTable("TypeOfDrive");

            entity.Property(e => e.Todid)
                .ValueGeneratedNever()
                .HasColumnName("TODID");
            entity.Property(e => e.Todname)
                .HasMaxLength(35)
                .IsFixedLength()
                .HasColumnName("TODName");

            entity.HasMany(d => d.CarVimFks).WithMany(p => p.TodidFks)
                .UsingEntity<Dictionary<string, object>>(
                    "CarTod",
                    r => r.HasOne<Car>().WithMany()
                        .HasForeignKey("CarVimFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_TOD_Cars"),
                    l => l.HasOne<TypeOfDrive>().WithMany()
                        .HasForeignKey("TodidFk")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Car_TOD_TypeOfDrive"),
                    j =>
                    {
                        j.HasKey("TodidFk", "CarVimFk");
                        j.ToTable("Car_TOD");
                        j.IndexerProperty<long>("TodidFk").HasColumnName("TODID_FK");
                        j.IndexerProperty<string>("CarVimFk")
                            .HasMaxLength(50)
                            .HasColumnName("CarVIM_FK");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
