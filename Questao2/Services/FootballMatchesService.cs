using Newtonsoft.Json;
using Questao2.Domain;
using Questao2.Services.Inteerfaces;

namespace Questao2.Services
{
    public class FootballMatchesService : IFootballMatchesService
    {
        public readonly string UrlBase = "https://jsonmock.hackerrank.com/api/football_matches";
        private readonly HttpClient _httpClient;

        public FootballMatchesService(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(UrlBase);
            _httpClient = httpClient;
        }
        public MatchResponse BuscarInformacoesTime(string time, int ano)
        {
            // Efetuando uma requisição à API
            string url = string.Format("{0}?year={1}&team1={2}", UrlBase, ano, time);
            return buscarIformacoesNaAPI(url);
        }

        public MatchResponse BuscarInformacoesTimePorPagina(string time, int ano, int pagina)
        {
            // Efetuando uma requisição à API
            string url = string.Format("{0}?year={1}&team1={2}&page={3}", UrlBase, ano, time, pagina);
            return buscarIformacoesNaAPI(url);
        }

        private MatchResponse buscarIformacoesNaAPI(string url)
        {
            HttpResponseMessage response = _httpClient.GetAsync(url).Result;
            // Deserializando a resposta da API
            return JsonConvert.DeserializeObject<MatchResponse>(response.Content.ReadAsStringAsync().Result);
        }

        public TeamDomain RetornarTotalGols(string time, int ano)
        {
            MatchResponse matchResponse = BuscarInformacoesTime(time, ano);
            int totalGols = 0;
            for (int i = 1; i <= matchResponse.total_pages; i++)
            {
                totalGols += BuscarInformacoesTimePorPagina(time, ano, i).data.Sum(x => int.Parse(x.team1goals));
            }
            bool timeExiste = matchResponse.data.Count() > 0;
            return new TeamDomain(time, totalGols, ano, timeExiste);
        }
    }
}
