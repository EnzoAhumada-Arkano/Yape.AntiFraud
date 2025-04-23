using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Infrastructure.Postgresql.Configuration
{
    public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");
            builder.HasKey(t => t.Id).HasName("id");
            builder.Property(t => t.TransactionExternalId).IsRequired().HasColumnName("transaction_external_id");
            builder.Property(t => t.SourceAccountId).IsRequired().HasColumnName("source_account_id");
            builder.Property(t => t.TargetAccountId).IsRequired().HasColumnName("target_account_id");
            builder.Property(t => t.TransferTypeId).IsRequired().HasColumnName("transfer_type_id");
            builder.Property(t => t.Value).IsRequired().HasColumnName("value");
            builder.Property(t => t.CreatedAt).IsRequired().HasColumnName("created_at");
            builder.Property(t => t.Status).IsRequired().HasColumnName("status");
        }
    }
}
