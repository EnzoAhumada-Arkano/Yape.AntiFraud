using Yape.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Application.Transaction.Queries
{
    public class GetTransactionByExternalIdQuery : IRequest<Yape.Domain.Entities.Transaction>
    {
        public Guid TransactionExternalId { get; set; }
    }
}
