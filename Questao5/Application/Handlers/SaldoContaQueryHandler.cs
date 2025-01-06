using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validation;
using Questao5.Infrastructure.Database.Repository.Interfaces;

namespace Questao5.Application.Handlers
{
    public class SaldoContaQueryHandler : IRequestHandler<SaldoContaQuery, SaldoContaResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentarRepository _movimentarRepository;

        public SaldoContaQueryHandler(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentarRepository movimentarRepository)
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentarRepository = movimentarRepository;
        }

        public async Task<SaldoContaResponse> Handle(SaldoContaQuery query, CancellationToken cancellationToken)
        {
            var contaCorrente = await _contaCorrenteRepository.GetContaByIdAsync(query.ContaCorrenteId);
            if (contaCorrente == null)
                throw new BusinessException("Conta corrente é inválida", "INVALID_ACCOUNT");

            if (!contaCorrente.Ativo)
                throw new BusinessException("Conta corrente é inativa", "INACTIVE_ACCOUNT");

            decimal creditos = await _movimentarRepository.GetSumByTipoAsync(query.ContaCorrenteId, TipoMovimento.CREDITO);
            decimal debitos = await _movimentarRepository.GetSumByTipoAsync(query.ContaCorrenteId, TipoMovimento.DEBITO);
            var saldo = creditos - debitos;

            return new SaldoContaResponse(
                contaCorrente.Numero,
                contaCorrente.Nome,
                DateTime.Now,
                saldo
            );
        }
    }
}
