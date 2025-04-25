using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yape.Domain.ApiClient
{
    public interface ITransactionApiClient
    {
        Task UpdateStatusAsync(Guid transactionExternalId, int status);
    }
}
