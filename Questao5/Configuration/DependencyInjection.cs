using MediatR;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Database.Repository.Interfaces;
using Questao5.Infrastructure.Database.Repository;

namespace Questao5.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
            services.AddScoped<IMovimentarRepository, MovimentarRepository>();
            services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();
            services.AddMediatR(typeof(MovimentarContaHandler).Assembly);

            return services;
        }
    }
}
