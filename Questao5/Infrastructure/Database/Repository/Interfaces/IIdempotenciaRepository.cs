using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repository.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task AddAsync(Idempotencia novoRegistroIdempotencia);
        Task<Idempotencia> GetIdempotenciaByIdAsync(Guid requestId);
    }
}
