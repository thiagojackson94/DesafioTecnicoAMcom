using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repository.Interfaces;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Sqlite;
using Dapper;

namespace Questao5.Infrastructure.Database.Repository
{
    public class ContaCorrenteRepository : IContaCorrenteRepository
    {
        private readonly DatabaseConfig _databaseConfig;
        public ContaCorrenteRepository(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }
        public async Task<ContaCorrente> GetContaByIdAsync(string id)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);

            var sql = @"SELECT IdContaCorrente, Numero, Nome, Ativo 
                    FROM ContaCorrente 
                    WHERE IdContaCorrente = @Id";

            var parameters = new
            {
                Id = id.ToUpper(),
            };

            var contaCorrente = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, parameters);

            return contaCorrente;
        }
    }
}
