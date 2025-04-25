using Yape.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Yape.Application.Transaction.Queries
{
    public class GetTransactionByExternalIdQuery : IRequest<TransactionRetrieve>
    {
        public Guid TransactionExternalId { get; set; }
    }
}
