using Questao2.Domain;

namespace Questao2.Services.Inteerfaces
{
    public interface IFootballMatchesService
    {
        MatchResponse BuscarInformacoesTime(string time, int ano);
        MatchResponse BuscarInformacoesTimePorPagina(string time, int ano, int pagina);
        TeamDomain RetornarTotalGols(string time, int ano);
    }
}
