using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Cliente.ObterCliente
{
    public interface IObterClienteUseCase
    {
        Task<ObterClienteOutput> Executar(string cpf);
    }
}