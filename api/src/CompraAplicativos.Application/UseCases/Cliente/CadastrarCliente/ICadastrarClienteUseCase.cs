using System.Threading.Tasks;

namespace CompraAplicativos.Application.UseCases.Cliente.CadastrarCliente
{
    public interface ICadastrarClienteUseCase
    {
        Task<CadastrarClienteOutput> Executar(CadastrarClienteInput input);
    }
}