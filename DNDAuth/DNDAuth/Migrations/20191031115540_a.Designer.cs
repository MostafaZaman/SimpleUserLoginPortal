﻿// <auto-generated />
using DNDAuth.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DNDAuth.Migrations
{
    [DbContext(typeof(DNDAuthDBContext))]
    [Migration("20191031115540_a")]
    partial class a
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DNDAuth.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ActivationCode");

                    b.Property<string>("ConfirmPassword");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("EmailID");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsEmailVerified");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("SecurityAnswer");

                    b.Property<string>("SecurityQues");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
