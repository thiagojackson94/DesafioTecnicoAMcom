using Newtonsoft.Json;
using NSubstitute;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Handlers;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using Questao5.Domain.Validation;
using Questao5.Infrastructure.Database.Repository.Interfaces;
using Xunit;

namespace Questao5.Tests
{
    public class MovimentarContaHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentarRepository _movimentarRepository;
        private readonly IIdempotenciaRepository _idempotenciaRepository;
        private readonly MovimentarContaHandler _handler;

        public MovimentarContaHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentarRepository = Substitute.For<IMovimentarRepository>();
            _idempotenciaRepository = Substitute.For<IIdempotenciaRepository>();
            _handler = new MovimentarContaHandler(_contaCorrenteRepository, _movimentarRepository, _idempotenciaRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnIdMovimentacao_WhenRequestIsOkAsync()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 100m,
                TipoMovimento = TipoMovimento.CREDITO.ToCode()
            };

            var contaCorrente = new ContaCorrente(123, "Katherine Sanchez", true);
            var movimentoId = Guid.NewGuid().ToString();

            _contaCorrenteRepository.GetContaByIdAsync(request.ContaCorrenteId.ToString()).Returns(Task.FromResult(contaCorrente));
            _movimentarRepository.AddAsync(Arg.Any<Movimento>()).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.IdMovimentacao);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenInvalidAccountAsync()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 500,
                TipoMovimento = TipoMovimento.CREDITO.ToCode(),
            };
            var idempotencia = new Idempotencia(request.RequestId, JsonConvert.SerializeObject(request), "3fa85f64-5717-4562-b3fc-2c963f66afa6");

            _contaCorrenteRepository.GetContaByIdAsync(request.ContaCorrenteId.ToString())
                .Returns(Task.FromResult<ContaCorrente>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Conta corrente é inválida ou inativa", exception.Mensagem);
            Assert.Equal("INVALID_ACCOUNT", exception.Tipo);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenInvalidValueAsync()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = -500,
                TipoMovimento = TipoMovimento.CREDITO.ToCode(),
            };

            var contaCorrente = new ContaCorrente(123, "Katherine Sanchez", true);

            _contaCorrenteRepository.GetContaByIdAsync(request.ContaCorrenteId.ToString())
                .Returns(Task.FromResult(contaCorrente));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Valor é inválido", exception.Mensagem);
            Assert.Equal("INVALID_VALUE", exception.Tipo);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenInvalidTypeAsync()
        {
            // Arrange
            var request = new MovimentarContaCommand
            {
                RequestId = Guid.NewGuid(),
                ContaCorrenteId = Guid.NewGuid(),
                Valor = 500,
                TipoMovimento = "ABC",
            };

            var contaCorrente = new ContaCorrente(123, "Katherine Sanchez", true);

            _contaCorrenteRepository.GetContaByIdAsync(request.ContaCorrenteId.ToString())
                .Returns(Task.FromResult(contaCorrente));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Tipo de movimento é inválido", exception.Mensagem);
            Assert.Equal("INVALID_TYPE", exception.Tipo);
        }
    }
}
