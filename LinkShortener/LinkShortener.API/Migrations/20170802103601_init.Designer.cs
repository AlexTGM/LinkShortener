﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using LinkShortener.API.Repository.Impl;

namespace LinkShortener.API.Migrations
{
    [DbContext(typeof(LinkShortenerContext))]
    [Migration("20170802103601_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("LinkShortener.API.Models.ShortLink", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("FullLink")
                        .IsRequired();

                    b.Property<string>("Key")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ShortLink");
                });
        }
    }
}
