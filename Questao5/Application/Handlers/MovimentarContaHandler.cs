using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using Questao5.Domain.Validation;
using Questao5.Infrastructure.Database.Repository.Interfaces;

namespace Questao5.Application.Handlers
{
    public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, MovimentarContaResponse>
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentarRepository _movimentarRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;

        public MovimentarContaHandler(
            IContaCorrenteRepository contaCorrenteRepository,
            IMovimentarRepository movimentarRepository,
            IIdempotenciaRepository idempotenciaRepository
            )
        {
            _contaCorrenteRepository = contaCorrenteRepository;
            _movimentarRepository = movimentarRepository;
            _idempotenciaRepository = idempotenciaRepository;
        }

        public async Task<MovimentarContaResponse> Handle(MovimentarContaCommand request, CancellationToken cancellationToken)
        {
            var idempotencia = await _idempotenciaRepository.GetIdempotenciaByIdAsync(request.RequestId);
            if (idempotencia != null)
            {
                return new MovimentarContaResponse(idempotencia.Resultado);
            }

            var contaCorrente = await _contaCorrenteRepository.GetContaByIdAsync(request.ContaCorrenteId.ToString());
            if (contaCorrente == null || !contaCorrente.Ativo)
                throw new BusinessException("Conta corrente é inválida ou inativa", "INVALID_ACCOUNT");

            if (request.Valor <= 0)
                throw new BusinessException("Valor é inválido", "INVALID_VALUE");

            if (request?.TipoMovimento?.ToTipoMovimento() != TipoMovimento.DEBITO && request?.TipoMovimento?.ToTipoMovimento() != TipoMovimento.CREDITO)
                throw new BusinessException("Tipo de movimento é inválido", "INVALID_TYPE");

            var movimento = new Movimento(
                request.ContaCorrenteId.ToString(),
                request.TipoMovimento.ToTipoMovimento(),
                request.Valor
            );

            await _movimentarRepository.AddAsync(movimento);

            var novoRegistroIdempotencia = new Idempotencia(
                request.RequestId,
                JsonConvert.SerializeObject(request),
                movimento.Id
            );
            await _idempotenciaRepository.AddAsync(novoRegistroIdempotencia);

            return new MovimentarContaResponse(movimento.Id);
        }
    }
}
