using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Application.Transaction.Commands
{
    public class PatchTransactionStatusCommand : IRequest<Guid>
    {
        public Guid TransactionExternalId { get; set; }
        public int Status { get; set; }
    }
}
