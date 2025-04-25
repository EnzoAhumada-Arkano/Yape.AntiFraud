using Yape.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;

namespace Yape.Application.Transaction.Queries
{
    public class GetAllTransactionsQuery : IRequest<List<TransactionRetrieve>> {}
}
