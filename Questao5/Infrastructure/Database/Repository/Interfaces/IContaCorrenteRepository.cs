using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repository.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetContaByIdAsync(string id);
    }
}
