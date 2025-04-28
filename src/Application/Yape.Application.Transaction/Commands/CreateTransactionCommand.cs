using Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Application.Transaction.Commands
{
    public class CreateTransactionCommand : TransactionCreate, IRequest<Guid>
    {

    }
}
