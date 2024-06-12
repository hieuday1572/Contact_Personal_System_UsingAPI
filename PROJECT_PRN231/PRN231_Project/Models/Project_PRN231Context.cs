using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231_Project.Models
{
    public partial class Project_PRN231Context : DbContext
    {
        public Project_PRN231Context()
        {
        }

        public Project_PRN231Context(DbContextOptions<Project_PRN231Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; } = null!;
        public virtual DbSet<ContactLabel> ContactLabels { get; set; } = null!;
        public virtual DbSet<Label> Labels { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contact");

                entity.Property(e => e.Company)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(300)
                    .IsFixedLength();

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Contacts)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Contact_User");
            });

            modelBuilder.Entity<ContactLabel>(entity =>
            {
                entity.ToTable("Contact_Label");

                entity.HasOne(d => d.Contact)
                    .WithMany(p => p.ContactLabels)
                    .HasForeignKey(d => d.ContactId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contact_Label_Contact");

                entity.HasOne(d => d.Label)
                    .WithMany(p => p.ContactLabels)
                    .HasForeignKey(d => d.LabelId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Contact_Label_Label");
            });

            modelBuilder.Entity<Label>(entity =>
            {
                entity.ToTable("Label");

                entity.Property(e => e.LabelName)
                    .HasMaxLength(200)
                    .IsFixedLength();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Labels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Label_User");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
