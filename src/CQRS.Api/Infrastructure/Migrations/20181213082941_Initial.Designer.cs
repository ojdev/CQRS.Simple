﻿// <auto-generated />
using CQRS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CQRS.Api.Infrastructure.Migrations
{
    [DbContext(typeof(CQRSDomainContext))]
    [Migration("20181213082941_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ChangeDetector.SkipDetectChanges", "true")
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687");
#pragma warning restore 612, 618
        }
    }
}
