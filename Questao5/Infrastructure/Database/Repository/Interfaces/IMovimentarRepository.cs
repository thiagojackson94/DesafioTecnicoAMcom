using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;

namespace Questao5.Infrastructure.Database.Repository.Interfaces
{
    public interface IMovimentarRepository
    {
        Task AddAsync(Movimento movimento);
        Task<decimal> GetSumByTipoAsync(string contaCorrenteId, TipoMovimento debito);
    }
}
