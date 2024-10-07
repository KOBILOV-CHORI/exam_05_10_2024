using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class DataContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }

    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).IsRequired().HasColumnName("Id");

            entity
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("FirstName");

            entity
                .Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("LastName");

            entity.Property(e => e.Email).IsRequired().HasMaxLength(255).HasColumnName("Email");

            entity.HasIndex(e => e.Email).IsUnique(); // Уникальность Email

            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20).HasColumnName("Phone");

            entity.Property(e => e.DateOfBirth).IsRequired().HasColumnName("DateOfBirth");

            entity.Property(e => e.HireDate).IsRequired().HasColumnName("HireDate");

            entity
                .Property(e => e.Position)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("Position");

            entity
                .Property(e => e.Salary)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasColumnName("Salary");

            entity.Property(e => e.DepartmentId).IsRequired().HasColumnName("DepartmentId");

            entity.Property(e => e.ManagerId).HasColumnName("ManagerId");

            entity
                .Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");

            entity.Property(e => e.Address).IsRequired().HasMaxLength(255).HasColumnName("Address");

            entity.Property(e => e.City).IsRequired().HasMaxLength(100).HasColumnName("City");

            entity.Property(e => e.Country).IsRequired().HasMaxLength(100).HasColumnName("Country");

            entity
                .Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("CreatedAt");

            entity
                .Property(e => e.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("UpdatedAt")
                .ValueGeneratedOnAddOrUpdate();
        });
    }
}
