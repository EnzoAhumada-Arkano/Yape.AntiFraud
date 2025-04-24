using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public Guid TransactionExternalId { get; set; } = Guid.NewGuid();
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public decimal Value { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int Status { get; set; } = 0;
    }
}
