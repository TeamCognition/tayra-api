﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tayra.Models.Catalog;

namespace Tayra.Models.Catalog.Migrations
{
    [DbContext(typeof(CatalogDbContext))]
    partial class CatalogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tayra.Models.Catalog.Identity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Created");

                    b.Property<int>("CreatedBy");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("LastModified");

                    b.Property<int?>("LastModifiedBy");

                    b.Property<string>("LastName")
                        .HasMaxLength(50);

                    b.Property<byte[]>("Password")
                        .IsRequired();

                    b.Property<byte[]>("Salt")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Identities");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.IdentityEmail", b =>
                {
                    b.Property<int>("IdentityId");

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<bool>("IsPrimary");

                    b.Property<DateTime?>("LastModified");

                    b.HasKey("IdentityId", "Email");

                    b.HasIndex("Email", "IsPrimary")
                        .IsUnique();

                    b.ToTable("IdentityEmails");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.LandingPageContact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailAddress");

                    b.Property<string>("Message");

                    b.Property<string>("Name");

                    b.Property<string>("PhoneNumber");

                    b.HasKey("Id");

                    b.ToTable("LandingPageContacts");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.LandingPageTry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("EmailAddress");

                    b.HasKey("Id");

                    b.ToTable("LandingPageTry");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.Tenant", b =>
                {
                    b.Property<byte[]>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128);

                    b.Property<string>("DisplayName")
                        .HasMaxLength(100);

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ServicePlan")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(10)")
                        .HasDefaultValueSql("'standard'");

                    b.Property<string>("Timezone")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("Key");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.TenantIdentity", b =>
                {
                    b.Property<byte[]>("TenantId");

                    b.Property<int>("IdentityId");

                    b.Property<DateTime>("Created");

                    b.Property<DateTime?>("LastModified");

                    b.HasKey("TenantId", "IdentityId");

                    b.HasIndex("IdentityId");

                    b.ToTable("TenantIdentities");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.TenantIntegration", b =>
                {
                    b.Property<byte[]>("TenantId");

                    b.Property<int>("Type");

                    b.Property<int>("SegmentId");

                    b.Property<DateTime>("Created");

                    b.Property<string>("InstallationId")
                        .HasMaxLength(125);

                    b.HasKey("TenantId", "Type", "SegmentId");

                    b.HasIndex("InstallationId");

                    b.ToTable("TenantIntegrations");
                });

            modelBuilder.Entity("Tayra.Models.Catalog.IdentityEmail", b =>
                {
                    b.HasOne("Tayra.Models.Catalog.Identity", "Identity")
                        .WithMany("Emails")
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tayra.Models.Catalog.TenantIdentity", b =>
                {
                    b.HasOne("Tayra.Models.Catalog.Identity", "Identity")
                        .WithMany()
                        .HasForeignKey("IdentityId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tayra.Models.Catalog.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tayra.Models.Catalog.TenantIntegration", b =>
                {
                    b.HasOne("Tayra.Models.Catalog.Tenant", "Tenant")
                        .WithMany()
                        .HasForeignKey("TenantId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
