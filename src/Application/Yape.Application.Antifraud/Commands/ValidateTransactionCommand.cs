using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Application.Antifraud.Commands
{
    public class ValidateTransactionCommand : IRequest<bool>
    {
        public Guid TransactionExternalId { get; set; }
    }
}
