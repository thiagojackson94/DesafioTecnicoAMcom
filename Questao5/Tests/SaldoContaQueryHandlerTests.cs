using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Validation;
using Questao5.Infrastructure.Database.Repository.Interfaces;
using Xunit;

namespace Questao5.Tests
{
    public class SaldoContaQueryHandlerTests
    {
        private readonly IContaCorrenteRepository _contaCorrenteRepository;
        private readonly IMovimentarRepository _movimentarRepository;
        private readonly SaldoContaQueryHandler _handler;

        public SaldoContaQueryHandlerTests()
        {
            _contaCorrenteRepository = Substitute.For<IContaCorrenteRepository>();
            _movimentarRepository = Substitute.For<IMovimentarRepository>();
            _handler = new SaldoContaQueryHandler(_contaCorrenteRepository, _movimentarRepository);
        }

        [Fact]
        public async Task Handle_ShouldReturnSaldo_WhenAccountIsValidAsync()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new SaldoContaQuery(contaCorrenteId);
            var contaCorrente = new ContaCorrente(123, "Katherine Sanchez", true);

            _contaCorrenteRepository.GetContaByIdAsync(contaCorrenteId).Returns(Task.FromResult(contaCorrente));
            _movimentarRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.CREDITO).Returns(Task.FromResult(1000m));
            _movimentarRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.DEBITO).Returns(Task.FromResult(500m));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Numero);
            Assert.Equal("Katherine Sanchez", result.Nome);
            Assert.Equal(500, result.Saldo);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenAccountIsInactiveAsync()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new SaldoContaQuery(contaCorrenteId);
            var contaCorrente = new ContaCorrente(123, "Katherine Sanchez", false);

            _contaCorrenteRepository.GetContaByIdAsync(contaCorrenteId).Returns(Task.FromResult(contaCorrente));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Conta corrente é inativa", exception.Mensagem);
            Assert.Equal("INACTIVE_ACCOUNT", exception.Tipo);
        }

        [Fact]
        public async Task Handle_ShouldReturn400_WhenAccountIsInvalidAsync()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new SaldoContaQuery(contaCorrenteId);

            _contaCorrenteRepository.GetContaByIdAsync(contaCorrenteId).Returns(Task.FromResult<ContaCorrente>(null));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(query, CancellationToken.None));
            Assert.Equal("Conta corrente é inválida", exception.Mensagem);
            Assert.Equal("INVALID_ACCOUNT", exception.Tipo);
        }

        [Fact]
        public async Task Handle_ShouldReturnZeroValue_WhenNotExistMovimentAsync()
        {
            // Arrange
            var contaCorrenteId = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
            var query = new SaldoContaQuery(contaCorrenteId);
            var contaCorrente = new ContaCorrente(123, "Katherine Sanchez", true);

            _contaCorrenteRepository.GetContaByIdAsync(contaCorrenteId).Returns(Task.FromResult(contaCorrente));
            _movimentarRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.CREDITO).Returns(Task.FromResult(0m));
            _movimentarRepository.GetSumByTipoAsync(contaCorrenteId, TipoMovimento.DEBITO).Returns(Task.FromResult(0m));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Numero);
            Assert.Equal("Katherine Sanchez", result.Nome);
            Assert.Equal(0m, result.Saldo);
        }
    }
}
