using Microsoft.Extensions.DependencyInjection;
using Questao2;
using Questao2.Services;
using Questao2.Services.Inteerfaces;
using System.Globalization;

public class Program
{
    public static void Main()
    {
        Console.Write("-- Questao 2 - Quantidade de Gols Por Time em um Ano --" + Environment.NewLine);

        var services = new ServiceCollection();
        ConfigureServices(services);
        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetService<IFootballMatchesService>();

        bool repetirPrograma;
        do
        {
            try
            {

                Console.Write("Entre com o Ano:");
                if (int.TryParse(Console.ReadLine(), out int ano) && ano < 1863)
                    throw new ArgumentException("Ano inválido");

                Console.Write("Entre com o Nome do time:");
                string time = Console.ReadLine();

                var matches = service.BuscarInformacoesTime(time, ano);

                int totalGoals = service.RetornarTotalGols(time, ano).TotalGols;

                Console.WriteLine("Team " + time + " scored " + totalGoals.ToString() + " goals in " + ano + Environment.NewLine);

                Console.WriteLine("Deseja pesquisar outro time? (s/n)");
                _ = char.TryParse(Console.ReadLine(), out char resp);

                repetirPrograma = resp.ToLower(CultureInfo.InvariantCulture).Equals('s');

                Console.WriteLine();

            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu uma falha: {e.Message}");
                Console.WriteLine("Deseja tentar novamente? (s/n)");
                _ = char.TryParse(Console.ReadLine(), out char resp);

                repetirPrograma = resp.ToLower(CultureInfo.InvariantCulture).Equals('s');
            }
        } while (repetirPrograma);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpClient<IFootballMatchesService, FootballMatchesService>();
    }
}