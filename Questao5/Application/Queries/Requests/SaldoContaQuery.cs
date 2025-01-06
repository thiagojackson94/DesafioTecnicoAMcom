using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class SaldoContaQuery : IRequest<SaldoContaResponse>
    {
        public string ContaCorrenteId { get; private set; }

        public SaldoContaQuery(string contaCorrenteId)
        {
            ContaCorrenteId = contaCorrenteId;
        }
    }
}
