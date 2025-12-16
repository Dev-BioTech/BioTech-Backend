using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using CommercialService.Domain.Entities;
using CommercialService.Domain.Enums;

namespace CommercialService.Domain.Interfaces;

public interface ICommercialRepository
{
    Task AddTransactionAsync(CommercialTransaction transaction, CancellationToken cancellationToken);
    Task<CommercialTransaction?> GetTransactionByIdAsync(long id, CancellationToken cancellationToken);

    Task<List<CommercialTransaction>> GetTransactionsAsync(int farmId, DateTime? fromDate, DateTime? toDate, TransactionType? type, CancellationToken cancellationToken);

    Task<List<TransactionAnimalDetail>> GetTransactionAnimalsAsync(long transactionId, CancellationToken cancellationToken);

    Task<List<TransactionProductDetail>> GetTransactionProductsAsync(long transactionId, CancellationToken cancellationToken);
}
