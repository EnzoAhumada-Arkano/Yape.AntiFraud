using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.ApiClient
{
    public interface IAntifraudApiClient
    {
        Task ValidateTransactionAsync(Guid transactionExternalId);
    }
}
