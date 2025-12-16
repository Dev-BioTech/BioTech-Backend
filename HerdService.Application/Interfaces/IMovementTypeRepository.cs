using HerdService.Domain.Entities;

namespace HerdService.Application.Interfaces;

public interface IMovementTypeRepository
{
    Task<MovementType> AddAsync(MovementType movementType, CancellationToken cancellationToken);
    Task<IEnumerable<MovementType>> GetAllAsync(CancellationToken cancellationToken);
    Task<MovementType?> GetByIdAsync(int id, CancellationToken cancellationToken);
}
