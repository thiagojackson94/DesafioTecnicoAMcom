namespace Questao5.Application.Commands.Responses
{
    public class MovimentarContaResponse
    {
        public string IdMovimentacao { get; private set; }

        public MovimentarContaResponse(string idMovimentacao) =>
            IdMovimentacao = idMovimentacao;

    }
}
