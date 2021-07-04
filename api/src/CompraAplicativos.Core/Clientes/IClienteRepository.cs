using System.Threading.Tasks;

namespace CompraAplicativos.Core.Clientes
{
    public interface IClienteRepository
    {
        Task Cadastrar(Cliente cliente);
        Task<bool> VerificarClienteExistePorCpf(string cpf);
    }
}
