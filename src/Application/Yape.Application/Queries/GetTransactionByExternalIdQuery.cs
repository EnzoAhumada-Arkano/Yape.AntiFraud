using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class GetTransactionByExternalIdQuery : IRequest<Transaction>
    {
        public Guid TransactionExternalId { get; set; }
    }
}
