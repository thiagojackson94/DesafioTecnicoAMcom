using Questao5.Domain.Enumerators;
using Questao5.Domain.Validation;

namespace Questao5.Domain.Extensions
{
    public static class TipoMovimentoExtension
    {
        private static readonly Dictionary<string, TipoMovimento> TipoMovimentoMap = new Dictionary<string, TipoMovimento>
        {
            { "C", TipoMovimento.CREDITO },
            { "D", TipoMovimento.DEBITO }
        };

        public static TipoMovimento ToTipoMovimento(this string tipoString)
        {
            if (TipoMovimentoMap.TryGetValue(tipoString, out var tipo))
            {
                return tipo;
            }
            else
            {
                throw new BusinessException($"Tipo de movimento é inválido", "INVALID_TYPE");
            }
        }
        public static string ToCode(this TipoMovimento tipo)
        {
            return tipo switch
            {
                TipoMovimento.CREDITO => "C",
                TipoMovimento.DEBITO => "D",
                _ => throw new ArgumentOutOfRangeException(nameof(tipo), tipo, null)
            };
        }
    }
}
