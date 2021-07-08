using CompraAplicativos.Consumer.DataAccess.Repositories;
using CompraAplicativos.Infrastructure.MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CompraAplicativos.Consumer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<DataAccess.MongoDB>();
                    services.AddSingleton<ICompraRepository, CompraRepository>();
                    services.AddSingleton<IProcessaCompraReceiver, ProcessaCompraReceiver>();
                    services.AddHostedService<Worker>();
                });
        }
    }
}
