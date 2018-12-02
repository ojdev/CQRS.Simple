using CQRS.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CQRS.Infrastructure.EntityConfigurations
{
    class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : Entity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(o => o.Id);
            builder.Ignore(o => o.DomainEvents);
            builder.Property<DateTimeOffset>("CreationTime").HasField("_creationTime").HasDefaultValueSql("GETDATE()").ValueGeneratedOnAdd();
            builder.Property<DateTimeOffset?>("LastUpdateTime").HasField("_lastUpdateTime");
            builder.Property<DateTimeOffset?>("DeletionTime");
            builder.Property<bool?>("IsDeleted").HasDefaultValue(false);
            builder.Property<byte[]>("RowVersion").IsRowVersion();
            builder.HasQueryFilter(o => EF.Property<bool>(o, "IsDeleted") == false);
        }
    }
}
